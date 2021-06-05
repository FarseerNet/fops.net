using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
using FOPS.Com.BuilderServer.Build;
using FS.DI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FOPS.Blazor.Background
{
    /// <summary>
    /// 运行构建
    /// </summary>
    public class RunBuildService : BackgroundService
    {
        private readonly IIocManager         _ioc;
        readonly         ILogger             _logger;

        public RunBuildService(IIocManager ioc)
        {
            _ioc                = ioc;
            _logger             = _ioc.Logger<RunBuildService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("开始执行队列");
            while (true)
            {
                try
                {
                    await _ioc.Resolve<IBuildOpr>().Build();
                }
                catch (Exception e)
                {
                    _logger.LogError(e,e.Message);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}