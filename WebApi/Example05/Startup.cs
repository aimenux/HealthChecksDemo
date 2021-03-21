using System;
using System.Collections.Generic;
using System.Linq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.HealthCheckers;

namespace WebApi.Example05
{
    public class Startup
    {
        private const int MaxHealthCheckRequests = 2;

        private const string HealthCheckLiveEndpoint = @"/healthchecks/live";
        private const string HealthCheckReadyEndpoint = @"/healthchecks/ready";

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
                .AddCheck<PingHealthChecker>(nameof(PingHealthChecker), tags: new List<string> {"ping"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck<RandomHealthChecker>(nameof(RandomHealthChecker), tags: new List<string> {"random"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "CpuChecker", check: () => HealthCheckResult.Healthy("OK"), tags: new List<string> {"cpu"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "DiskChecker", check: () => HealthCheckResult.Degraded("UNK"), tags: new List<string> {"disk"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "MemoryChecker", check: () => HealthCheckResult.Unhealthy("KO"), tags: new List<string> {"memory"}, timeout: TimeSpan.FromSeconds(1));

            services.AddHealthChecksUI(setupSettings: settings =>
            {
                settings.SetApiMaxActiveRequests(MaxHealthCheckRequests);
                settings.SetEvaluationTimeInSeconds(TimeSpan.FromSeconds(10).Seconds);
                settings.AddHealthCheckEndpoint($"{ExampleName} [Liveness]", HealthCheckLiveEndpoint);
                settings.AddHealthCheckEndpoint($"{ExampleName} [Readiness]", HealthCheckReadyEndpoint);
                settings.SetMinimumSecondsBetweenFailureNotifications(TimeSpan.FromSeconds(30).Seconds);
            }).AddInMemoryStorage();
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
                endpoints.MapHealthChecks(HealthCheckLiveEndpoint, LiveOptions);
                endpoints.MapHealthChecks(HealthCheckReadyEndpoint, ReadyOptions);
                endpoints.MapHealthChecksUI();
            });
        }

        public HealthCheckOptions LiveOptions { get; } = new()
        {
            Predicate = setup => setup.Tags.Contains("ping"),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };

        public HealthCheckOptions ReadyOptions { get; } = new()
        {
            Predicate = setup => !setup.Tags.Contains("ping"),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };

        private string GetInstrumentationKey()
        {
            const string key = @"Serilog:WriteTo:2:Args:instrumentationKey";
            var instrumentationKey = Configuration.GetValue<string>(key);
            return instrumentationKey;
        }
    }
}
