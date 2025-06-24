using System;
using System.IO;
using System.Threading;

using RakNet;

using SkySaga.Game;

var keepRunning = true;

Console.CancelKeyPress += delegate
{
    keepRunning = false;
};

if (!File.Exists("RakNet.dll"))
{
    Console.WriteLine("""
        The RakNet DLL is missing.
        Press any key to continue . . .
        """);

    Console.ReadKey();

    return;
}

try
{
    _ = new RakString();
}
catch
{
    Console.WriteLine("""
        RakNet DLL issue.
        Most likely the provided DLL wasn't build with the C# wrapper file.
        Press any key to continue . . .
        """);

    Console.ReadKey();

    return;
}

ushort port = 42069;

using var server = new Server("Something about penguins\0", port);

if (!server.Start())
{
    Console.WriteLine("""
        Failed to start server.
        Press any key to continue . . .
        """);

    Console.ReadKey();

    return;
}

Console.WriteLine($"Server has started on port {port}.");

while (keepRunning)
{
    server.Tick();

    Thread.Sleep(30);
}

Console.WriteLine("""
        Server has stopped.
        Press any key to continue . . .
        """);

Console.ReadKey();