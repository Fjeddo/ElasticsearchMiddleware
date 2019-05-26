using System.Diagnostics;
using System.Threading.Tasks;
using ES.LoggingMiddleware.Common.Models;
using log4net;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware
{
    internal class ElasticLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;

        public ElasticLoggingMiddleware(RequestDelegate next, ILog log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = new InternalLogger(_log, context);

            logger.LogRequestStarting();

            await _next(context);

            logger.LogRequestEnding();
        }

        private class InternalLogger
        {
            private readonly ILog _log;
            private readonly HttpContext _context;
            private readonly Stopwatch _logOverheadStopwatch;
            private readonly Stopwatch _requestStopwatch;
            private dynamic _logContext;

            public InternalLogger(ILog log, HttpContext context)
            {
                _log = log;
                _context = context;
                _logOverheadStopwatch = new Stopwatch();
                _requestStopwatch = new Stopwatch();
            }

            public void LogRequestStarting()
            {
                var requestStarting = new RequestStarting(new HttpRequestContext(_context), out _logContext);

                _logOverheadStopwatch.Start();
                _log.Info(requestStarting);
                _logOverheadStopwatch.Stop();

                _requestStopwatch.Restart();
            }

            public void LogRequestEnding()
            {
                _requestStopwatch.Stop();
                var requestTimeElapsed = _requestStopwatch.ElapsedMilliseconds;

                var requestFinished =
                    new RequestFinished(requestTimeElapsed, _logContext, new HttpResponseContext(_context));

                _logOverheadStopwatch.Start();
                _log.Info(requestFinished);
                _logOverheadStopwatch.Stop();

                //_log.Debug($"DEBUG {DateTimeOffset.Now.ToString(Constants.DateTimeOffsetFormat)}: Request/response logging overhead {_logOverheadStopwatch.ElapsedMilliseconds} ms ({_logContext.CorrelationId})");

            }
        }
    }
}
