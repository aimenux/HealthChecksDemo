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

namespace WebApi.Example04
{
    public class Startup
    {
        private const string HealthCheckPattern = @"/healthchecks";
        private static readonly string HealthCheckEndpointUrl = @$"https://localhost:44313{HealthCheckPattern}";

        private string ExampleName => GetType().Namespace?.Split('.').LastOrDefault();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ExampleName, Version = "v1" });
            });

            services.AddHealthChecks()
                .AddCheck<PingHealthChecker>(nameof(PingHealthChecker), tags: new List<string> {"ping"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck<RandomHealthChecker>(nameof(RandomHealthChecker), tags: new List<string> {"random"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "CpuChecker", check: () => HealthCheckResult.Healthy("OK"), tags: new List<string> {"cpu"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "DiskChecker", check: () => HealthCheckResult.Degraded("KO"), tags: new List<string> {"disk"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "MemoryChecker", check: () => HealthCheckResult.Unhealthy("KO"), tags: new List<string> {"memory"}, timeout: TimeSpan.FromSeconds(1));

            services.AddHealthChecksUI(setupSettings: settings =>
            {
                settings.SetEvaluationTimeInSeconds(TimeSpan.FromSeconds(10).Seconds);
                settings.AddHealthCheckEndpoint(ExampleName, HealthCheckEndpointUrl);
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
                endpoints.MapHealthChecks(HealthCheckPattern, Options);
                endpoints.MapHealthChecksUI();
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
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };
    }
}
