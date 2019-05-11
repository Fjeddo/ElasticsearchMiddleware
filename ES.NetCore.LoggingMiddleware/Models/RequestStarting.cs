using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware.Models
{
    public class RequestStarting
    {
        public dynamic RequestContext { get; }

        public RequestStarting(ref Stopwatch stopwatch, HttpContext context, out dynamic ctx)
        {
            RequestContext = new
            {
                CorrelationId = Guid.NewGuid(),
                context.Request.Path,
                context.Request.Host,
                context.Request.IsHttps,
                context.Request.Method,
                context.Request.Headers
            };

            stopwatch?.Reset();
            stopwatch?.Start();

            ctx = RequestContext;
        }

        public override string ToString()
        {
            return $"Request started {RequestContext.Path} at {DateTimeOffset.UtcNow} ({RequestContext.CorrelationId})";
        }
    }
}