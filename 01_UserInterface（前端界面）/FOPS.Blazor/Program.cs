using FS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FOPS.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        FarseerApplication.Run<Startup>("FOPS.Blazor").Initialize();
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).UseWindsorContainerServiceProvider().ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseKestrel().UseStartup<Startup>();
        });
    }
}