using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FS.Core.Exception;
using FS.Core.Net;
using FS.DI;
using FS.Extends;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FOPS.Blazor
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private          IIocManager     _ioc;
        public ExceptionMiddleware(RequestDelegate next,IIocManager ioc)
        {
            _next    = next;
            this._ioc = ioc;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string requestContent = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(httpContext.Request.ContentType))
                {
                    // 允许其他管道重复读流
                    httpContext.Request.EnableBuffering();
                    requestContent = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                    // 将流定位到初始位置，以让mvc能读到完整的入参
                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                }

                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                var http = httpContext.Request.IsHttps ? "s" : "";
                var lst = new List<string>
                {
                    $"Path：http{http}://{httpContext.Request.Host}{httpContext.Request.Path}",
                    $"Method：{httpContext.Request.Method}",
                    $"ContentType：{httpContext.Request.ContentType}",
                    $"Body：{requestContent}",
                };
                
                _ioc.Logger<ExceptionMiddleware>().LogError(string.Join(",",lst));
            }
        }
    }
}