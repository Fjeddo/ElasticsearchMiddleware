using System;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware.Models
{
    public class RequestFinished
    {
        public dynamic RequestContext { get; }
        public dynamic ResponseContext { get; }
        public long ElapsedMilliseconds { get; }
        public string Timestamp { get; }

        public RequestFinished(long elapsedMilliseconds, dynamic requestContext, HttpContext httpContext)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            RequestContext = requestContext;
            ResponseContext = new
            {
                httpContext.Response.StatusCode,
                httpContext.Response.ContentType
            };

            Timestamp = DateTimeOffset.Now.ToString(ElasticLoggingMiddleware.DateTimeOffsetFormat);
        }

        public override string ToString()
        {
            return $"Request finished with status {ResponseContext.StatusCode} in {ElapsedMilliseconds} ms {RequestContext.Path} at {Timestamp} ({RequestContext.CorrelationId})";
        }
    }
}