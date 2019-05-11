using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware.Models
{
    public class RequestFinished
    {
        public dynamic RequestContext { get; }
        public dynamic ResponseContext { get; }
        public long ElapsedMilliseconds { get; }

        public RequestFinished(ref Stopwatch stopwatch, dynamic requestContext, HttpContext httpContext)
        {
            stopwatch?.Stop();
            ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1;

            RequestContext = requestContext;
            ResponseContext = new
            {
                httpContext.Response.StatusCode,
                httpContext.Response.ContentType
            };
        }

        public override string ToString()
        {
            return $"Request finished with status {ResponseContext.StatusCode} in {ElapsedMilliseconds} ms {RequestContext.Path} at {DateTimeOffset.UtcNow} ({RequestContext.CorrelationId})";
        }
    }
}