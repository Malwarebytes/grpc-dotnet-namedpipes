using System;
using System.Threading;
using System.Threading.Tasks;
using EADomain;
using Google.Protobuf.WellKnownTypes;
using GrpcDotNetNamedPipes;

namespace Client
{
    internal class Program
    {
        const string PLUGIN_PIPE_NAME = "Malwarebytes.Plugin1";

        static void Main(string[] args)
        {
            //Local event pump so even a console app can have a synchronization context
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

                //Call more than one engine function in parallel.
                //Also test reentrancy when calling the same function from separate threads
                var message = new MyMessage();


                message.Duration = 5000;
                var task1 = engine.Function1Async(message).ResponseAsync;
                message.Duration = 2000;
                var task2 = engine.Function2Async(message).ResponseAsync;
                message.Duration = 1000;
                var task3 = engine.Function1Async(message).ResponseAsync;

                Console.WriteLine("Waiting for all tasks to finish...");
                Task.WaitAll(task1, task2, task3);

                Console.WriteLine("Press key to exit client...");
                Console.ReadKey();
            }
        }
    }
}
