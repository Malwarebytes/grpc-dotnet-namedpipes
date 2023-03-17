using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EADomain;
using Grpc.Core;

namespace Client
{
    internal class Plugin : IPlugin.IPluginBase
    {
        public override Task<PluginInitialization> Initialize(PluginPolicy policy, ServerCallContext context)
        {
            Console.WriteLine("DownloadFile() called in the server.");

            //_logger.LogInformation("Saying hello to {Name}", request.Name);
            return Task.FromResult(new PluginInitialization 
            {
                SupportedCommands = { "command1", "command2" }
            });
        }
    }
}
