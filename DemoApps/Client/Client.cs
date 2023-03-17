using System;
using System.Threading;
using System.Threading.Tasks;
using EADomain;
using GrpcDotNetNamedPipes;

namespace Client
{
    internal class Program
    {
        const string PLUGIN_PIPE_NAME = "Malwarebytes.Plugin1";

        static void Main(string[] args)
        {
            AsyncPump.Run(AsyncMain);
        }

        static async Task AsyncMain()
        {
            var cts = new CancellationTokenSource();

            //gRPC client to make calls into the Engine
            var channel = new NamedPipeChannel(".", "Malwarebytes.PipeServerName");
            var engine = new IEAEngine.IEAEngineClient(channel);

            //gRPC server to receive calls from the Engine
            using (var client = new NamedPipeServer(PLUGIN_PIPE_NAME))
            {
                IPlugin.BindService(client.ServiceBinder, new Plugin());
                client.Start();

                var clientPipe = new ClientPipe
                {
                    PipeName = PLUGIN_PIPE_NAME
                };
                
                await engine.NotifyPluginConnectedAsync(clientPipe);

                Console.WriteLine("Press key to exit client...");
                Console.ReadKey();
            }
        }
    }
}
