using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware.Models
{
    public class RequestStarting
    {
        public dynamic RequestContext { get; }
        public string Timestamp { get; }

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

            Timestamp = DateTimeOffset.UtcNow.ToString(ElasticLoggingMiddleware.DateTimeOffsetFormat);
        }
        
        public override string ToString()
        {
            return $"Request started {RequestContext.Path} at {Timestamp} ({RequestContext.CorrelationId})";
        }
    }
}