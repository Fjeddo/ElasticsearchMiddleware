using System.Diagnostics;
using System.IO;
using System.Reflection;
using ES.NetCore.LoggingMiddleware.Models;
using log4net;
using Microsoft.AspNetCore.Builder;

namespace ES.NetCore.LoggingMiddleware
{
    public static class ESLoggingMiddleware
    {
        private static ILog _log;

        public static IApplicationBuilder UseESWithLog4Net(this IApplicationBuilder app, string loggerName = "eslogger")
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            _log = string.IsNullOrEmpty(loggerName)
                ? LogManager.GetLogger(Assembly.GetEntryAssembly(), typeof(ESLoggingMiddleware))
                : LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);

            app.Use(async (httpContext, next) =>
            {
                var stopwatch = new Stopwatch();

                _log.Info(new RequestStarting(ref stopwatch, httpContext, out var context));

                await next();

                _log.Info(new RequestFinished(ref stopwatch, context, httpContext));
            });

            return app;
        }
    }
}
