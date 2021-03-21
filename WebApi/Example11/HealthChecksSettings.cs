using System.Collections.Generic;

namespace WebApi.Example11
{
    public class HealthChecksSettings
    {
        public const string HealthCheckLiveEndpoint = @"/healthchecks/live";

        public const string HealthCheckReadyEndpoint = @"/healthchecks/ready";

        public string SqlServerStorageConnectionString { get; set; }

        public SqlServerHealthChecksSettings SqlServerHealthChecks { get; set; }
    }

    public class SqlServerHealthChecksSettings : List<SqlServerSettings>
    {
    }

    public class SqlServerSettings
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }
    }
}
