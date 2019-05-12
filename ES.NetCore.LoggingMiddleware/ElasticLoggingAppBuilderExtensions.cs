using System;
using System.IO;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Builder;

namespace ES.NetCore.LoggingMiddleware
{
    public static class ElasticLoggingAppBuilderExtensions
    {
        private const string DefaultLoggerName = "eslogger";
        private const string DefaultConfigFileName = "log4net.config";

        private static ILog _log;

        public static IApplicationBuilder UseElasticWithLog4Net(this IApplicationBuilder app) => 
            UseElasticWithLog4Net(app, DefaultLoggerName, GetDefaultFileInfo());

        public static IApplicationBuilder UseElasticWithLog4Net(this IApplicationBuilder app, FileInfo configFile) => 
            UseElasticWithLog4Net(app, DefaultLoggerName, configFile);

        public static IApplicationBuilder UseElasticWithLog4Net(this IApplicationBuilder app, string loggerName) => 
            UseElasticWithLog4Net(app, loggerName, GetDefaultFileInfo());

        public static IApplicationBuilder UseElasticWithLog4Net(this IApplicationBuilder app, string loggerName, FileInfo configFile)
        {
            ConfigureLog4Net(configFile);
            _log = GetLogger(loggerName);

            app.UseMiddleware<ElasticLoggingMiddleware>(_log);

            return app;
        }

        private static void ConfigureLog4Net(FileInfo configFile)
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(repository, configFile);
        }

        private static ILog GetLogger(string loggerName) =>
            string.IsNullOrEmpty(loggerName)
                ? LogManager.GetLogger(Assembly.GetEntryAssembly(), typeof(ElasticLoggingAppBuilderExtensions))
                : LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);
        
        private static FileInfo GetDefaultFileInfo() =>
            new FileInfo(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new InvalidOperationException("Could not get entry assembly location"),
                    DefaultConfigFileName));
    }
}
