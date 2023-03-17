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
        private List<IPlugin.IPluginClient> _plugins;

        public Engine(/*ILogger<GreeterService> logger*/)
        {
            //_logger = logger;
        }

        public override Task<ClientNotify> NotifyPluginConnected(ClientPipe clientPipe, ServerCallContext context)
        {
            Console.WriteLine($"Plugin connected. Named pipe for connections is {clientPipe.PipeName}");

            //Add plugin and create named pipe client to send request to the plugin
            var channel = new NamedPipeChannel(".", clientPipe.PipeName);
            _pluginChannels.Add(channel);
            var plugin = new IPlugin.IPluginClient(channel);
            _plugins.Add(plugin);

            return Task.FromResult(new ClientNotify());
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
