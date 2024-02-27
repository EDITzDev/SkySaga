using System;
using System.Text;
using System.Collections;

using RakNet;

namespace SkySaga.Game;

public static class BitStreamExtensions
{
    public static int ReadMessageId(this BitStream bitStream)
    {
        bitStream.Read(out byte messageIdPart);

        if (messageIdPart != byte.MaxValue)
            return messageIdPart;

        bitStream.Read(out messageIdPart);

        return byte.MaxValue - (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM + messageIdPart;
    }

    public static void WritePacketId(this BitStream bitStream, PacketId packetIdEnum)
    {
        var packetId = (byte)packetIdEnum;

        var packetIdPart = (byte)(packetId - (byte.MaxValue + 1 - (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM));

        if (packetId + (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM >= byte.MaxValue)
        {
            bitStream.Write(byte.MaxValue);
            packetIdPart++;
        }

        bitStream.Write(packetIdPart);
    }

    public static string ReadString(this BitStream bitStream)
    {
        var hasData = bitStream.ReadBit();

        if (!hasData)
            return string.Empty;

        var largeLength = bitStream.ReadBit();

        if (largeLength)
            throw new NotImplementedException();

        var lengthData = new byte[1];

        if (!bitStream.ReadBits(lengthData, 8u))
            return string.Empty;

        var length = lengthData[0];

        var stringData = new byte[length];

        if (!bitStream.ReadBits(stringData, length * 8u))
            return string.Empty;

        return Encoding.UTF8.GetString(stringData);
    }

    public static void WriteString(this BitStream bitStream, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            bitStream.Write0();
        }
        else
        {
            bitStream.Write1();

            var bytes = Encoding.UTF8.GetBytes(value);

            var length = bytes.Length;

            if (length < 255)
            {
                bitStream.Write0();
                bitStream.Write((byte)length);
            }
            else
            {
                bitStream.Write1();
                bitStream.Write(length);
            }

            bitStream.WriteBits(bytes, (uint)length * 8u, true);
        }
    }

    public static void WriteGuid(this BitStream bitStream, Guid value)
    {
        if (value == Guid.Empty)
        {
            bitStream.Write0();
            return;
        }

        bitStream.Write1();

        var guidBytes = value.ToByteArray();

        bitStream.Write(guidBytes, (uint)guidBytes.Length);
    }

    public static void WriteParameterFlags(this BitStream bitStream, int count, params int[] flags)
    {
        var data = new byte[16];

        var bitArray = new BitArray(count);

        foreach (var flag in flags)
            bitArray.Set(flag, true);

        bitArray.CopyTo(data, 0);

        for (int i = 0; i < count / 32; i++)
            Array.Reverse(data, i * 4, 4);

        bitStream.WriteBits(data, (uint)count);
    }
}