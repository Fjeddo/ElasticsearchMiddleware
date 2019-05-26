namespace ES.LoggingMiddleware.Common
{
    public interface IHttpResponseContext
    {
        int StatusCode { get; }
        string ContentType { get; }
    }
}