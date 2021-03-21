using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Example10
{
    public static class Extensions
    {
        public static IHealthChecksBuilder AddSqlServers(
            this IHealthChecksBuilder builder,
            SqlServerHealthChecksSettings sqlServerHealthChecksSettings,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            return sqlServerHealthChecksSettings.Aggregate(builder,
                (current, sqlServerSettings) => current.AddSqlServer(name: sqlServerSettings.Name,
                    connectionString: sqlServerSettings.ConnectionString, tags: tags, timeout: timeout));
        }
    }
}
