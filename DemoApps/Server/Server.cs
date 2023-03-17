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
                IEAEngine.BindService(server.ServiceBinder, new Engine());
                server.Start();
                Console.WriteLine("Waiting for connection. Press key to exit server...");
                Console.ReadKey();
            }
        }
    }
}
