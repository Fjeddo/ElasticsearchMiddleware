using System.Collections.Generic;

namespace ES.LoggingMiddleware.Common
{
    public interface IHttpRequestContext
    {
        string Path { get; }
        HostString Host { get; }
        bool IsHttps { get; }
        string Method { get; }
        Dictionary<string, string[]> Headers { get; }
    }

    public class HostString
    {
        public HostString(string host, int? port, string value, bool hasValue)
        {
            Host = host;
            Port = port;
            Value = value;
            HasValue = hasValue;
        }

        public bool HasValue { get; }
        public string Host { get; }
        public int? Port { get; }
        public string Value { get; }
    }

    public interface IHttpResponseContext
    {
        int StatusCode { get; }
        string ContentType { get; }
    }
}
   

    

