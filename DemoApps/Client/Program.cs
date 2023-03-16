using System;
using System.Threading.Tasks;
using EADomain;
using GrpcDotNetNamedPipes;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var channel = new NamedPipeChannel(".", "Malwarebytes.PipeServerName");
            var client = new Downloader.DownloaderClient(channel);

            var request = new DownloadRequest
            {
                Url = "url",
                AutoRedirect = true
            };

            var response = await client.DownloadFileAsync(request);

            Console.WriteLine($"Response: {response.FilePath}");
            Console.ReadKey();
        }
    }
}
