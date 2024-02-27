using System.Runtime.InteropServices;

namespace SmilegateAuth.Packet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal class PacketHeader
{
    public const byte Magic = 0xF1;

    private byte Start = Magic;

    public ushort Length;
    public ushort Id;
}