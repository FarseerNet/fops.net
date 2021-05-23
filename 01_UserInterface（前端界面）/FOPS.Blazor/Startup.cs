using FOPS.Blazor.Background;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FOPS.Com.BuilderServer;
using FOPS.Com.DockerServer;
using FOPS.Com.FssServer;
using FOPS.Com.K8SServer;
using FOPS.Com.MetaInfoServer;
using FS.Cache.Redis;
using FS.Core;
using FS.Data;
using FS.DI;
using FS.ElasticSearch;
using FS.Mapper;
using FS.Modules;
using FS.MQ.RedisStream;

namespace FOPS.Blazor
{
    [DependsOn(
        typeof(FarseerCoreModule),
        typeof(MapperModule),
        typeof(RedisModule),
        typeof(RedisStreamModule),
        typeof(DataModule),
        typeof(ElasticSearchModule),
        typeof(FssModule),
        typeof(K8SModule),
        typeof(BuilderModule),
        typeof(DockerModule),
        typeof(MetaInfoModule))]
    public class Startup: FarseerModule
    {
        public Startup()
        {
            
        }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public new IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIocManager>(FS.DI.IocManager.Instance.Resolve<IIocManager>());
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHostedService<RunBuildService>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}