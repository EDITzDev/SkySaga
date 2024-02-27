using System.Runtime.InteropServices;

namespace SmilegateAuth;

internal static class Util
{
    public static byte[] ToArray<T>(this T structure) where T : class
    {
        var size = Marshal.SizeOf<T>();

        var data = new byte[size];

        var ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(structure, ptr, true);
        Marshal.Copy(ptr, data, 0, size);

        Marshal.FreeHGlobal(ptr);

        return data;
    }

    public static T? ToStructure<T>(this byte[] data) where T : class
    {
        var size = Marshal.SizeOf<T>();
        var ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(data, 0, ptr, size);

        var structure = Marshal.PtrToStructure<T>(ptr);

        Marshal.FreeHGlobal(ptr);

        return structure;
    }
}