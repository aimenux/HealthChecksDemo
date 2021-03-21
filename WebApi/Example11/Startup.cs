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

namespace WebApi.Example11
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<HealthChecksDocumentFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ExampleName, Version = "v1" });
            });

            services.AddHealthChecks()
                .AddSqlServers(GetSqlServerHealthChecksSettings(), new List<string> {"sqlServer"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck<PingHealthChecker>(nameof(PingHealthChecker), tags: new List<string> {"ping"}, timeout: TimeSpan.FromSeconds(1))
                .AddCheck<RandomHealthChecker>(nameof(RandomHealthChecker), tags: new List<string> {"random"}, timeout: TimeSpan.FromSeconds(1));

            services.AddHealthChecksUI().AddSqlServerStorage(GetSqlServerStorageConnectionString());
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
                endpoints.MapHealthChecks(HealthChecksSettings.HealthCheckLiveEndpoint, LiveOptions);
                endpoints.MapHealthChecks(HealthChecksSettings.HealthCheckReadyEndpoint, ReadyOptions);
                endpoints.MapHealthChecksUI();
            });
        }

        public HealthCheckOptions LiveOptions { get; } = new()
        {
            Predicate = setup => setup.Name.Equals(nameof(PingHealthChecker)),
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
            Predicate = setup => !setup.Name.Equals(nameof(PingHealthChecker)),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };

        private string GetSqlServerStorageConnectionString()
        {
            var sqlServerStorageConnectionString = Configuration
                .GetSection(nameof(HealthChecksSettings))
                .Get<HealthChecksSettings>()
                .SqlServerStorageConnectionString;

            return sqlServerStorageConnectionString;
        }

        private SqlServerHealthChecksSettings GetSqlServerHealthChecksSettings()
        {
            var sqlServerHealthChecksSettings = Configuration
                .GetSection(nameof(HealthChecksSettings))
                .Get<HealthChecksSettings>()
                .SqlServerHealthChecks;

            return sqlServerHealthChecksSettings;
        }
    }
}
