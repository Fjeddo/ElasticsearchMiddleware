using System.Collections.Generic;
using System.Linq;
using ES.LoggingMiddleware.Common;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware
{
    internal class HttpRequestContext : IHttpRequestContext
    {
        private readonly HttpContext _context;

        public HttpRequestContext(HttpContext context)
        {
            _context = context;
        }

        public string Path => _context.Request.Path;

        public ES.LoggingMiddleware.Common.HostString Host =>
            new ES.LoggingMiddleware.Common.HostString(
                _context.Request.Host.Host,
                _context.Request.Host.Port,
                _context.Request.Host.Value,
                _context.Request.Host.HasValue);

        public bool IsHttps => _context.Request.IsHttps;

        public string Method => _context.Request.Method;

        public Dictionary<string, string[]> Headers =>
            _context.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray());
    }
}