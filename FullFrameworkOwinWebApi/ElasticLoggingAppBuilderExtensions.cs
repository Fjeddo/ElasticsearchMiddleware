using System;
using System.IO;
using System.Reflection;
using log4net;
using Owin;

namespace FullFrameworkOwinWebApi
{
    public static class ElasticLoggingAppBuilderExtensions
    {
        private const string DefaultLoggerName = "eslogger";
        private const string DefaultConfigFileName = "log4net.config";

        private static ILog _log;

        public static IAppBuilder UseElasticWithLog4Net(this IAppBuilder app)
        {
            ConfigureLog4Net(GetDefaultFileInfo());
            _log = GetLogger(DefaultLoggerName);

            app.Use<ElasticLoggingMiddleware>(_log);

            return app;
        }

        private static void ConfigureLog4Net(FileInfo configFile)
        {
            var repository = LogManager.GetRepository();
            log4net.Config.XmlConfigurator.Configure(repository, configFile);
        }

        private static ILog GetLogger(string loggerName) =>
            string.IsNullOrEmpty(loggerName)
                ? LogManager.GetLogger(Assembly.GetExecutingAssembly(), typeof(ElasticLoggingAppBuilderExtensions))
                : LogManager.GetLogger(Assembly.GetExecutingAssembly(), loggerName);

        private static FileInfo GetDefaultFileInfo() =>
            new FileInfo(
                Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? throw new InvalidOperationException("Could not get app domain assembly location"),
                    DefaultConfigFileName));
    }
}