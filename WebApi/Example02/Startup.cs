using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.HealthCheckers;

namespace WebApi.Example02
{
    public class Startup
    {
        private string ExampleName => GetType().Namespace?.Split('.').LastOrDefault();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(GetInstrumentationKey());

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ExampleName, Version = "v1" });
            });

            services.AddHealthChecks()
                .AddCheck<PingHealthChecker>(nameof(PingHealthChecker), timeout: TimeSpan.FromSeconds(1))
                .AddCheck<RandomHealthChecker>(nameof(RandomHealthChecker), timeout: TimeSpan.FromSeconds(1));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/", Options);
            });
        }

        public HealthCheckOptions Options { get; } = new()
        {
            Predicate = _ => true,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = WriteHealthCheckResponse
        };

        private static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport healthReport)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("OverallStatus", healthReport.Status.ToString()),
                new JProperty("TotalChecksDuration", healthReport.TotalDuration.TotalSeconds.ToString("0:0.00")),
                new JProperty("DependencyHealthChecks", new JObject(healthReport.Entries.Select(entry =>
                    new JProperty(entry.Key, new JObject(
                        new JProperty("Status", entry.Value.Status.ToString()),
                        new JProperty("Duration", entry.Value.Duration.TotalSeconds.ToString("0:0.00")),
                        new JProperty("Exception", entry.Value.Exception?.Message),
                        new JProperty("Data", new JObject(entry.Value.Data.Select(dicData =>
                            new JProperty(dicData.Key, dicData.Value))))
                    ))
                )))
            );

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        private string GetInstrumentationKey()
        {
            const string key = @"Serilog:WriteTo:2:Args:instrumentationKey";
            var instrumentationKey = Configuration.GetValue<string>(key);
            return instrumentationKey;
        }
    }
}
