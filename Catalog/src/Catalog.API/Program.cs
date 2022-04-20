using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).ConfigureKestrel(options =>
           {
               options.Listen(IPAddress.Any, 5012, listenOptions =>
               {
                   listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
               });
           }).UseStartup<Startup>();
        }
    }
}
