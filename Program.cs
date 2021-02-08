using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casino.Log;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using NLog.Web;

namespace Casino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogConfiguration.SetupLogConfiguration(args);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}
