using ES.LoggingMiddleware.Common;
using Microsoft.AspNetCore.Http;

namespace ES.NetCore.LoggingMiddleware
{
    internal class HttpResponseContext : IHttpResponseContext
    {
        private readonly HttpContext _context;

        public HttpResponseContext(HttpContext context)
        {
            _context = context;
        }

        public int StatusCode => _context.Response.StatusCode;
        public string ContentType => _context.Response.ContentType;
    }
}