﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
        }

        [HttpGet("status")]
        public async Task<string> GetHealthStatusAsync()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            _logger.LogTrace("Health report: {@report}", report);

            return report.Status.ToString();
        }
    }
}
