using System.Runtime.InteropServices;

namespace SmilegateAuth.Packet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal class LoginRequest : PacketHeader
{
    public int Unknown;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Unknown2 = null!;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
    public string Username = null!;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Password = null!;

    public override string ToString()
    {
        return $"{nameof(Unknown)}: {Unknown}, " +
               $"{nameof(Unknown2)}: {Unknown2}, " +
               $"{nameof(Username)}: {Username}, " +
               $"{nameof(Password)}: {Password}";
    }
}