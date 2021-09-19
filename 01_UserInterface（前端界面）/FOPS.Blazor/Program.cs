using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FOPS.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FarseerApplication.Run<Startup>("FOPS.Blazor").Initialize(service => service.AddConsole());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).UseWindsorContainerServiceProvider().ConfigureWebHostDefaults(webBuilder =>
            {
                //Setup a HTTP/2 endpoint without TLS.
                webBuilder.UseKestrel().UseStartup<Startup>();
            });
        }
    }
}