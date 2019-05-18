using System.Threading.Tasks;
using log4net;
using Microsoft.Owin;

namespace FullFrameworkOwinWebApi
{
    public class ElasticLoggingMiddleware : OwinMiddleware
    {
        private readonly ILog _log;

        public ElasticLoggingMiddleware(OwinMiddleware next, ILog log) : base(next)
        {
            _log = log;
        }

        public override async Task Invoke(IOwinContext context)
        {
            _log.Info("START Test from full framework");

            await Next.Invoke(context);

            _log.Info("END Test from full framework");
        }
    }
}