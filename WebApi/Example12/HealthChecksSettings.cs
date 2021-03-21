namespace WebApi.Example12
{
    public class HealthChecksSettings
    {
        public const string HealthCheckLiveEndpoint = @"/healthchecks/live";

        public const string HealthCheckReadyEndpoint = @"/healthchecks/ready";

        public string SqlServerStorageConnectionString { get; set; }
    }
}
