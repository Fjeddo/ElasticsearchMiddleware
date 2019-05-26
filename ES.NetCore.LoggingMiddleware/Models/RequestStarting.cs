using System;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware.Models
{
    public class RequestStarting
    {
        public dynamic RequestContext { get; }
        public string Timestamp { get; }

        public RequestStarting(HttpContext context, out dynamic ctx)
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

            ctx = RequestContext;

            Timestamp = DateTimeOffset.Now.ToString(ElasticLoggingMiddleware.DateTimeOffsetFormat);
        }
        
        public override string ToString()
        {
            return $"Request started {RequestContext.Path} at {Timestamp} ({RequestContext.CorrelationId})";
        }
    }
}