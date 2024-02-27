using System.Runtime.InteropServices;

namespace SmilegateAuth.Packet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal class LoginReply : PacketHeader
{
    public LoginReply()
    {
        Id = (ushort)PacketId.LoginReply;
        Length = (ushort)Marshal.SizeOf(this);
    }

    // Results
    // -2 - DB Query error(-2)
    // -1 - ID does not exist
    //  0 - no error
    //  1 - The password is incorrect
    //  2 - The following are not allowed IP
    //  3 - The administrator has prohibited this conduct these actions. \n\tPlease contact our customer service at email [USER_BLOCKED]
    //  4 - Sorry, for now SkySaga is not available in your region. \t\n [COUNTRY_BLOCKED]
    //  5 - PASSWORD LOCKED. \n\tPlease contact our customer service at email [PWD_FAIL_BLOCKED]
    //  6 - [INVALID_SPOT_CODE]
    //  7 - canceled account[WITHDRAW_MEMBER]
    //  8 - maintenance[under maintenance]
    //  9 - Account not approved for the Closed Beta Test.[NOT_CBT_MEMBER]
    // 10 - The administrator has prohibited this conduct these actions.\n\tPlease contact our customer service at email [BAN_USER]

    public int Result;

    public int Unknown;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    private byte[] GAP = new byte[8];

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
    public string Username = null!;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string Token = null!;
}