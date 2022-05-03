using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Application.Build.Build;
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
        private readonly IIocManager _ioc;
        readonly         ILogger     _logger;

        public RunBuildService(IIocManager ioc)
        {
            _ioc    = ioc;
            _logger = _ioc.Logger<RunBuildService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);
            //var threadCount                  = Environment.ProcessorCount - 1;
            //if (threadCount < 1) threadCount = 1;
            var threadCount = 1;
            _logger.LogInformation($"开启构建队列，{threadCount}个线程");

            for (int i = 0; i < threadCount; i++)
            {
                _ = Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            await _ioc.Resolve<BuildApp>().Build();
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, e.Message);
                        }
                        await Task.Delay(1000, stoppingToken);
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }
    }
}