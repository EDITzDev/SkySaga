using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using SmilegateAuth;
using SmilegateAuth.Packet;

var packetBuffer = new byte[1024];

var tcpListener = new TcpListener(IPAddress.Any, 10106);

tcpListener.Start();

while (true)
{
    var tcpClient = tcpListener.AcceptTcpClient();

    var networkStream = tcpClient.GetStream();

    var readLength = networkStream.Read(packetBuffer, 0, packetBuffer.Length);

    // We need at least 5 bytes to process a packet.
    if (readLength < 5 || packetBuffer[0] != PacketHeader.Magic)
        continue;

    var packetHeader = packetBuffer.ToStructure<PacketHeader>();

    if (packetHeader is null || packetHeader.Length != readLength)
        continue;

    // There is only one packet id we're supposed to handle.
    if (packetHeader.Id != (ushort)PacketId.LoginRequest)
        continue;

    var loginRequest = packetBuffer.ToStructure<LoginRequest>();

    if (loginRequest is null)
        continue;

    Debug.WriteLine(loginRequest, nameof(LoginRequest));

    var loginReply = new LoginReply();

    loginReply.Result = loginRequest.Username != "EDITz" || loginRequest.Password != "EDITz" ? 1 : 0;

    loginReply.Username = loginRequest.Username;
    loginReply.Token = Guid.NewGuid().ToString();

    var loginReplyData = loginReply.ToArray();

    Debug.WriteLine(Convert.ToHexString(loginReplyData), nameof(LoginReply));

    networkStream.Write(loginReplyData);

    tcpClient.Close();
}