using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.HealthCheckers
{
    public class RandomHealthChecker : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            var next = RandomNumberGenerator.GetInt32(100, 500);

            await Task.Delay(next, cancellationToken);

            var result = next switch
            {
                var n when (n % 2 == 0) => HealthCheckResult.Healthy(),
                var n when (n % 5 == 0) => HealthCheckResult.Degraded(),
                _ => HealthCheckResult.Unhealthy()
            };

            return result;
        }
    }
}
