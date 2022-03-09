using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleService
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
         .AddEnvironmentVariables()
         .Build();

        public static void Main(string[] args)
        {
            string appVersion = "1.0.0";

            Log.Logger = new LoggerConfiguration().Destructure.UsingAttributes().ReadFrom.Configuration(Configuration)
                            .Enrich.WithProperty("Version", appVersion)
                            .WriteTo.File(new JsonFormatter(), "log.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();
            try
            {
                Log.Debug("Starting Payment Safe web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddSerilog();
                });
    }
}
