using System;
using EADomain;
using GrpcDotNetNamedPipes;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var server = new NamedPipeServer("Malwarebytes.PipeServerName"))
            {
                Downloader.BindService(server.ServiceBinder, new DownloaderService());
                server.Start();
                Console.WriteLine("Press key to exit server...");
                Console.ReadKey();
            }
        }
    }
}
