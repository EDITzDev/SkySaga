using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Binarysharp.MSharp;

namespace BundledFileExporter;

internal class Program
{
    static void Main()
    {
        if (!File.Exists("version.txt") || !File.Exists("SkySaga.exe"))
        {
            Debug.WriteLine($"version.txt or SkySaga.exe are missing.");
            return;
        }

        var versionData = File.ReadAllLines("version.txt");

        if (versionData.Length != 2)
        {
            Debug.WriteLine($"Unknown version.txt format.");
            return;
        }

        var versionNumber = versionData[0];
        var versionHash = versionData[1];

        nint address;
        bool encryptedName;
        bool encryptedData;

        if (versionNumber == "10414" && versionHash == "b511ad7f2fd042c6be8303a3b220184d4a85b658")
        {
            address = 0xc87f22;
            encryptedName = false;
            encryptedData = true;
        }
        else if (versionNumber == "14163" && versionHash == "f87fa467b9681e653c67eefaaf425fca865f4bdc")
        {
            address = 0xcb9f96;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "20015" && versionHash == "e83aa318dc0b8c4a89870ae4918cb0f804b400c3")
        {
            address = 0xd19654;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "25516" && versionHash == "d18b262fb4513389548ac0853758d12ec836856e")
        {
            address = 0xe9b3db;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "27094" && versionHash == "d1107d47355b4809cf98d436b5a60ca92e75ab06")
        {
            address = 0xe6edd4;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "30968" && versionHash == "9d8c08dc21aa2daae905b93edd6af3aa0eb4d9da")
        {
            address = 0xeed35a;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "36546" && versionHash == "2e14d8cad2ab735aee0c7a9bc7c4ef5591d234e2")
        {
            address = 0x12ec276;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "36731" && versionHash == "b2e36b9687b0898df9fb6953c3f3d42117c3ce1d")
        {
            address = 0x12d1282;
            encryptedName = true;
            encryptedData = true;
        }
        else if (versionNumber == "38328" && versionHash == "99ca6c78acfab268a58ed39b4db8ff0782165a28")
        {
            address = 0x14b9c4b;
            encryptedName = false;
            encryptedData = true;
        }
        else
        {
            Debug.WriteLine($"Unknown version or hash.");
            return;
        }

        if (!ExportBundledFiles(address, encryptedName, encryptedData))
        {
            Debug.WriteLine($"Failed to export bundled files.");
            return;
        }
    }

    internal static bool ExportBundledFiles(nint address, bool encryptedName, bool encryptedData)
    {
        using var process = Process.Start("SkySaga.exe", "allowim=1•devimip=localhost•manport=5164•ssl_active=0•useAnalytics=0•allowthreading=0•multiApp=1");

        if (process is null)
            return false;

        Thread.Sleep(500);

        using var memorySharp = new MemorySharp(process);

        var xorKey = memorySharp.Read<uint>(address);

        var dataOffset = memorySharp.Read<nint>(address + 0xc);

        var dataCheck = memorySharp.Read<uint>(dataOffset, false);

        if (dataCheck != 0xffffffff)
        {
            process.Kill();

            return false;
        }

        var fileCount = memorySharp.Read<int>(address + 0x78);

        if (fileCount <= 0)
        {
            process.Kill();

            return false;
        }

        var fileSizes = memorySharp.Read<int>(address + 0x7c, fileCount);

        var fileNames = new string[fileCount];

        var fileNameOffset = address + 0x7c + (4 * fileCount);

        for (var i = 0; i < fileCount; i++)
        {
            var fileName = memorySharp.ReadString(fileNameOffset);

            fileNameOffset += fileName.Length + 1;

            fileNames[i] = encryptedName ? DecryptFileName(xorKey, fileName) : fileName;
        }

        var fileDataOffset = dataOffset + 4;

        for (var i = 0; i < fileCount; i++)
        {
            var fileData = memorySharp.Read<byte>(fileDataOffset, fileSizes[i], false);

            if (encryptedData)
                DecryptFileData(xorKey, ref fileData);

            var savePath = Path.Combine("Bundled", fileNames[i]);

            var saveDirectory = Path.GetDirectoryName(savePath);

            if (saveDirectory is not null && !Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            File.WriteAllBytes(savePath, fileData);

            fileDataOffset += fileData.Length;
        }

        process.Kill();

        return true;
    }

    internal static string DecryptFileName(uint key, string value)
    {
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
            stringBuilder.Append((char)(value[i] - (byte)(2 * key & 7) - (byte)(key & 7)));

        return stringBuilder.ToString();
    }

    internal static void DecryptFileData(uint key, ref byte[] value)
    {
        var key1 = 2 * key;

        for (var i = 0; i < value.Length; i++)
        {
            value[i] = (byte)(key1 + (key1.Byte(1) ^ value[i]));

            key1.Ror(1);
        }
    }
}