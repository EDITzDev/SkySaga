namespace BundledFileExporter;

internal static class Util
{
    internal static byte Byte(this ref uint value, int pos)
    {
        return (byte)((value >> (8 * pos)) & 0xff);
    }

    internal static void Ror(this ref uint value, int count)
    {
        value = (value >> 1) | (value << (32 - count));
    }
}