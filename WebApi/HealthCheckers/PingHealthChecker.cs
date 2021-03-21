using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.HealthCheckers
{
    public class PingHealthChecker : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
