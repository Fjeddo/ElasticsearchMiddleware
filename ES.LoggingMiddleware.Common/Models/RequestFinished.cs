using System;

namespace ES.LoggingMiddleware.Common.Models
{
    public class RequestFinished
    {
        public dynamic RequestContext { get; }
        public dynamic ResponseContext { get; }
        public long ElapsedMilliseconds { get; }
        public string Timestamp { get; }

        public RequestFinished(long elapsedMilliseconds, dynamic requestContext, IHttpResponseContext httpContext)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            RequestContext = requestContext;
            ResponseContext = new
            {
                httpContext.StatusCode,
                httpContext.ContentType
            };

            Timestamp = DateTimeOffset.Now.ToString(Constants.DateTimeOffsetFormat);
        }

        public override string ToString()
        {
            return $"Request finished with status {ResponseContext.StatusCode} in {ElapsedMilliseconds} ms {RequestContext.Path} at {Timestamp} ({RequestContext.CorrelationId})";
        }
    }
}