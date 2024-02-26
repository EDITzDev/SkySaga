using System;
using System.IO;
using System.Buffers.Binary;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace WWiseCatalogue;

internal class Program
{
    internal static Dictionary<uint, string> _idTable = new();
    internal static Dictionary<uint, string> _hashTable = new();

    internal static void Main()
    {
        if (!File.Exists("WWiseCatalogue.json"))
            return;

        var jsonObject = JsonNode.Parse(File.ReadAllText("WWiseCatalogue.json"));

        if (jsonObject is null)
            return;

        var bankList = jsonObject["bankList"]?.AsArray();

        if (bankList is null)
            return;

        foreach (var bankItem in bankList)
        {
            var value = bankItem?.GetValue<string>();

            if (value is null)
                continue;

            _hashTable.Add(Util.ComputeCrc32(value), value);

            var tempValue = Path.GetFileNameWithoutExtension(value);

            _hashTable.Add(Util.ComputeCrc32(tempValue), tempValue);
        }

        var catalogueList = jsonObject["catalogue"]?.AsObject();

        if (catalogueList is null)
            return;

        foreach (var catalogueItem in catalogueList)
        {
            var value = catalogueItem.Value?["id"]?.GetValue<string>();

            if (value is null)
                continue;

            var id = uint.Parse(value);

            _idTable.TryAdd(id, catalogueItem.Key);
        }

        foreach (var filePath in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.wem"))
        {
            var relativeFilePath = filePath.Substring(Environment.CurrentDirectory.Length + 1);

            var fileName = GetFileNameFromHash(relativeFilePath);

            if (fileName is null)
                fileName = Path.GetFileNameWithoutExtension(relativeFilePath);

            var saveDirectory = Path.Combine(Environment.CurrentDirectory, "Extracted");

            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            var wemFilePath = Path.Combine(saveDirectory, $"{fileName}.wem");

            File.Copy(filePath, wemFilePath);
        }

        foreach (var filePath in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.bnk", SearchOption.AllDirectories))
        {
            var relativeFilePath = filePath.Substring(Environment.CurrentDirectory.Length + 1);

            var fileName = GetBankFileNameFromHash(relativeFilePath);

            fileName ??= Path.GetFileNameWithoutExtension(relativeFilePath);

            ExtractBankFile(relativeFilePath, fileName);
        }
    }

    internal static string? GetFileNameFromHash(string filePath)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        if (!uint.TryParse(fileNameWithoutExtension, out var fileNameId))
            return null;

        return _idTable.TryGetValue(fileNameId, out var fileName) ? fileName : null;
    }

    internal static string? GetBankFileNameFromHash(string filePath)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        var fileNameHash = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt32(Convert.FromHexString(fileNameWithoutExtension)));

        return _hashTable.TryGetValue(fileNameHash, out var fileName) ? fileName : null;
    }

    internal static void ExtractBankFile(string filePath, string fileName)
    {
        var chunks = new Dictionary<string, byte[]>();

        using (var br = new BinaryReader(File.OpenRead(filePath)))
        {
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                var type = new string(br.ReadChars(4));

                chunks.Add(type, br.ReadBytes(br.ReadInt32()));
            }
        }

        if (!chunks.TryGetValue("DIDX", out var dataIndexChunk))
            return;

        var dataIndexes = new List<(uint Id, int Offset, int Size)>();

        using (var br = new BinaryReader(new MemoryStream(dataIndexChunk)))
        {
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                dataIndexes.Add((br.ReadUInt32(), br.ReadInt32(), br.ReadInt32()));
            }
        }

        if (!chunks.TryGetValue("DATA", out var dataChunk))
            return;

        var dataSpan = dataChunk.AsSpan();

        var saveDirectory = Path.Combine(Environment.CurrentDirectory, "Extracted");

        var fileDirectory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(fileDirectory))
            saveDirectory = Path.Combine(saveDirectory, fileDirectory);

        saveDirectory = Path.Combine(saveDirectory, fileName);

        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);

        foreach (var dataIndex in dataIndexes)
        {
            if (!_idTable.TryGetValue(dataIndex.Id, out var idName))
                idName = dataIndex.Id.ToString();

            var wemFilePath = Path.Combine(saveDirectory, $"{idName}.wem");

            using var binaryWriter = new BinaryWriter(File.OpenWrite(wemFilePath));

            binaryWriter.Write(dataSpan.Slice(dataIndex.Offset, dataIndex.Size));
        }
    }
}