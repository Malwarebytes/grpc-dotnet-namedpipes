using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EADomain;
using Grpc.Core;

namespace Server
{
    internal class DownloaderService : Downloader.DownloaderBase
    {
        //private readonly ILogger<DownloaderService> _logger;

        public DownloaderService(/*ILogger<GreeterService> logger*/)
        {
            //_logger = logger;
        }

        public override Task<DownloadReply> DownloadFile(DownloadRequest request, ServerCallContext context)
        {
            Console.WriteLine("DownloadFile() called in the server.");

            //_logger.LogInformation("Saying hello to {Name}", request.Name);
            return Task.FromResult(new DownloadReply 
            {
                HttpStatus = 200,
                FilePath = "foo"
            });
        }
    }
}
