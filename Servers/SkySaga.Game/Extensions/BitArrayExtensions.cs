using System.Collections;

namespace SkySaga.Game.Extensions;

public static class BitArrayExtensions
{
    public static bool IsEqual(this BitArray bitArray, BitArray other)
    {
        if (bitArray is null || other is null)
            return false;

        if (bitArray.Length != other.Length)
            return false;

        for (var i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i] != other[i])
                return false;
        }

        return true;
    }
}