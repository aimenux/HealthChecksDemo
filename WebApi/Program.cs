using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebApi
{
    public static class Program
    {
        public static void Main() => CreateHostBuilder().Build().Run();

        public static IHostBuilder CreateHostBuilder()
        {
            var (startupType, jsonFile) = Examples.GetStartupConfiguration();

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(jsonFile)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(startupType);
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, bool.TrueString);
                })
                .UseJsonConfigSerilog();
        }

        public static IHostBuilder UseJsonConfigSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                Serilog.Debugging.SelfLog.Enable(Console.Error);
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            });
        }

        public static IHostBuilder ConfigureAppConfiguration(
            this IHostBuilder hostBuilder,
            string jsonFile = null)
        {
            return string.IsNullOrWhiteSpace(jsonFile)
                ? hostBuilder
                : hostBuilder.ConfigureAppConfiguration(builder => builder.AddJsonFile(jsonFile, optional: false, reloadOnChange: true));
        }
    }
}
