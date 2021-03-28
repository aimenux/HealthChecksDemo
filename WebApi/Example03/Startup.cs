using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebApi.Example03
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
                .AddCheck(name: "CpuChecker", check: () => HealthCheckResult.Healthy("OK"), tags: new List<string> {"cpu"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "DiskChecker", check: () => HealthCheckResult.Degraded("UNK"), tags: new List<string> {"disk"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck(name: "MemoryChecker", check: () => HealthCheckResult.Unhealthy("KO"), tags: new List<string> {"memory"}, timeout: TimeSpan.FromSeconds(1));
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
                endpoints.MapHealthChecks("/options1", Options1);
                endpoints.MapHealthChecks("/options2", Options2);
                endpoints.MapHealthChecks("/options3", Options3);
                endpoints.MapHealthChecks("/options4", Options4);
            });
        }

        public HealthCheckOptions Options1 { get; } = new()
        {
            Predicate = setup => setup.Tags.Contains("cpu")
        };

        public HealthCheckOptions Options2 { get; } = new()
        {
            Predicate = setup => setup.Tags.Contains("disk")
        };

        public HealthCheckOptions Options3 { get; } = new()
        {
            Predicate = setup => setup.Tags.Contains("memory")
        };

        public HealthCheckOptions Options4 { get; } = new()
        {
            Predicate = _ => true,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        };

        private string GetInstrumentationKey()
        {
            const string key = @"Serilog:WriteTo:2:Args:instrumentationKey";
            var instrumentationKey = Configuration.GetValue<string>(key);
            return instrumentationKey;
        }
    }
}
