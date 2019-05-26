using System;

namespace ES.LoggingMiddleware.Common.Models
{
    internal class Constants
    {
        internal const string DateTimeOffsetFormat = "yyyy-MM-dd HH:mm:ss.fffzzz";
    }

    public class RequestStarting
    {
        public dynamic RequestContext { get; }
        public string Timestamp { get; }

        public RequestStarting(IHttpRequestContext context, out dynamic ctx)
        {
            RequestContext = new
            {
                CorrelationId = Guid.NewGuid(),
                context.Path,
                context.Host,
                context.IsHttps,
                context.Method,
                context.Headers
            };

            ctx = RequestContext;

            Timestamp = DateTimeOffset.Now.ToString(Constants.DateTimeOffsetFormat);
        }
        
        public override string ToString()
        {
            return $"Request started {RequestContext.Path} at {Timestamp} ({RequestContext.CorrelationId})";
        }
    }
}