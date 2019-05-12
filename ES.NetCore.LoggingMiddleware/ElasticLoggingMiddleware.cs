using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ES.NetCore.LoggingMiddleware.Models;
using log4net;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware
{
    internal class ElasticLoggingMiddleware
    {
        internal const string DateTimeOffsetFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";

        private readonly RequestDelegate _next;
        private readonly ILog _log;

        public ElasticLoggingMiddleware(RequestDelegate next, ILog log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            var debuggSW = new Stopwatch();

            debuggSW.Start();
            _log.Info(new RequestStarting(ref stopwatch, context, out var logContext));
            debuggSW.Stop();

            await _next(context);

            debuggSW.Start();
            _log.Info(new RequestFinished(ref stopwatch, logContext, context));
            debuggSW.Stop();

            _log.Debug($"DEBUG {DateTimeOffset.UtcNow.ToString(DateTimeOffsetFormat)}: Request/response logging overhead {debuggSW.ElapsedMilliseconds} ms ({logContext.CorrelationId})");
        }
    }
}