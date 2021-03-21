using System.Collections.Generic;

namespace WebApi.Example10
{
    public class HealthChecksSettings
    {
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
