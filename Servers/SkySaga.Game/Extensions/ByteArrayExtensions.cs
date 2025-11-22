using System;

namespace SkySaga.Game.Extensions;

/// <summary>
/// Extension methods for byte array bit manipulation and parsing
/// </summary>
public static class ByteArrayExtensions
{
    /// <summary>
    /// Extracts a bit-packed value from a byte array using big-endian bit ordering (MSB first).
    /// Useful for parsing bit-packed network data where values are not byte-aligned.
    /// </summary>
    /// <param name="data">The byte array containing the bit-packed data</param>
    /// <param name="bitOffset">The starting bit position (0 = MSB of first byte)</param>
    /// <param name="bitLength">The number of bits to extract</param>
    /// <returns>The extracted value as an integer. Returns 0 if offset exceeds data bounds.</returns>
    public static int ExtractBits(this byte[] data, int bitOffset, int bitLength)
    {
        int value = 0;

        for (int i = 0; i < bitLength; i++)
        {
            int byteIndex = (bitOffset + i) / 8;
            int bitIndex = 7 - ((bitOffset + i) % 8);

            if (byteIndex >= data.Length)
                return 0;

            int bit = (data[byteIndex] >> bitIndex) & 1;
            value = (value << 1) | bit;
        }

        return value;
    }
}
