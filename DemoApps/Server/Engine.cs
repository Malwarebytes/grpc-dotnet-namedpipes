using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EADomain;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcDotNetNamedPipes;

namespace Server
{
    internal class Engine : IEAEngine.IEAEngineBase
    {
        //private readonly ILogger<Engine> _logger;

        private List<NamedPipeChannel> _pluginChannels = new List<NamedPipeChannel>();
        private List<IPlugin.IPluginClient> _plugins = new List<IPlugin.IPluginClient>();

        public Engine(/*ILogger<GreeterService> logger*/)
        {
            //_logger = logger;
        }

        public override async Task<Empty> NotifyPluginConnected(ClientPipe clientPipe, ServerCallContext context)
        {
            Console.WriteLine($"Plugin connected. Named pipe for connections is {clientPipe.PipeName}");

            //Add plugin and create named pipe client to send request to the plugin
            var channel = new NamedPipeChannel(".", clientPipe.PipeName);
            _pluginChannels.Add(channel);
            var plugin = new IPlugin.IPluginClient(channel);
            _plugins.Add(plugin);

            //Initialize plugin
            var response = await plugin.InitializeAsync(new PluginPolicy
                {
                    Policy = "rtp: true",
                });

            Console.WriteLine("Plugin initialized. Supported commands:");
            foreach (var command in response.SupportedCommands)
            {
                Console.WriteLine($"\t{command}");
            }

            return new Empty();
        }

        //public override Task<DownloadReply> DownloadFile(DownloadRequest request, ServerCallContext context)
        //{
        //    Console.WriteLine("DownloadFile() called in the server.");

        //    //_logger.LogInformation("Saying hello to {Name}", request.Name);
        //    return Task.FromResult(new DownloadReply 
        //    {
        //        HttpStatus = 200,
        //        FilePath = "foo"
        //    });
        //}
    }
}
