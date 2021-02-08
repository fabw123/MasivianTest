using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using NLog.Web;

namespace Casino.Log
{
    public class NLogConfiguration
    {
        public static void SetupLogConfiguration(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main function");
                var accessKey = Environment.GetEnvironmentVariable(EnvironmentSettings.CW_ACCESS_KEY);
                var secretKey = Environment.GetEnvironmentVariable(EnvironmentSettings.CW_SECRET_KEY);
                var config = new LoggingConfiguration();
                var awsTarget = new AWSTarget()
                {
                    LogGroup = "NLog.Config",
                    Region = "us-east-2",
                    Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey)
                };
                config.AddTarget("aws", awsTarget);
                config.LoggingRules.Add(new LoggingRule("*", GetLogLevel(), awsTarget));
                LogManager.Configuration = config;
                LogManager.Configuration.Variables["minLevel"] = Environment.GetEnvironmentVariable(EnvironmentSettings.LOG_MIN_LEVEL);
                LogManager.ReconfigExistingLoggers();
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in init");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .UseNLog();

        private static NLog.LogLevel GetLogLevel()
        {
            string minLogLevel = Environment.GetEnvironmentVariable(EnvironmentSettings.LOG_MIN_LEVEL);
            switch (minLogLevel)
            {
                case "Trace":
                    return NLog.LogLevel.Trace;
                case "Debug":
                    return NLog.LogLevel.Debug;
                case "Warn":
                    return NLog.LogLevel.Warn;
                case "Error":
                    return NLog.LogLevel.Error;
                case "Fatal":
                    return NLog.LogLevel.Fatal;
                case "Off":
                    return NLog.LogLevel.Off;
                case "Info":
                default:
                    return NLog.LogLevel.Info;
            }
        }
    }
}
