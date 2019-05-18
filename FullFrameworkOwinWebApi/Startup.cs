using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FullFrameworkOwinWebApi.Startup))]
namespace FullFrameworkOwinWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            app.UseElasticWithLog4Net();
            app.UseWebApi(httpConfiguration);
        }
    }
}
