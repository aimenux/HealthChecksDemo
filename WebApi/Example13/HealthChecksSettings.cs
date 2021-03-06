using System;
using HealthChecks.UI.Core;

namespace WebApi.Example13
{
    public class HealthChecksSettings
    {
        public RequestCatcherSettings RequestCatcherSettings { get; set; }

        public MicrosoftTeamsSettings MicrosoftTeamsSettings { get; set; }
    }

    public class RequestCatcherSettings : IWebhookSettings
    {
        public string Name { get; set; } = "RequestCatcher";

        public string Url { get; set; } = "!PUT-YOUR-REQUEST-CATCHER-URL-HERE!";

        public string FailurePayload { get; set; } = "{ message: \"[[LIVENESS]] service is failed. [[FAILURE]]\"}";

        public string RestorePayload { get; set; } = "{ message: \"[[LIVENESS]] service is recovered. There is 0 healthcheck failing.\"}";

        public Func<UIHealthReport, bool> ShouldNotifyFunc { get; set; } = null;
        
        public Func<UIHealthReport, string> CustomMessageFunc { get; set; } = null;
        
        public Func<UIHealthReport, string> CustomDescriptionFunc { get; set; } = null;
    }

    public class MicrosoftTeamsSettings : IWebhookSettings
    {
        public string Name { get; set; } = "MicrosoftTeams";

        public string Url { get; set; } = "!PUT-YOUR-MICROSOFT-TEAMS-URL-HERE!";

        public string FailurePayload { get; set; } = "{\r\n \"@type\": \"MessageCard\",\r\n \"themeColor\": \"c60035\",\r\n \"title\": \"[[LIVENESS]] is failed.\",\r\n \"text\": \"[[FAILURE]]\",\r\n \"potentialAction\": [\r\n {\r\n \"@type\": \"OpenUri\",\r\n \"name\": \"Learn More\",\r\n \"targets\": [\r\n { \"os\": \"default\", \"uri\": \"https://localhost:44313/healthchecks-ui\" }\r\n ]\r\n }\r\n ]\r\n}";

        public string RestorePayload { get; set; } = "{\r\n \"@type\": \"MessageCard\",\r\n \"themeColor\": \"00c66a\",\r\n \"title\": \"[[LIVENESS]] is recovered.\",\r\n \"text\": \"There is 0 healthcheck failing.\",\r\n \"potentialAction\": [\r\n {\r\n \"@type\": \"OpenUri\",\r\n \"name\": \"Learn More\",\r\n \"targets\": [\r\n { \"os\": \"default\", \"uri\": \"https://localhost:44313/healthchecks-ui\" }\r\n ]\r\n }\r\n ]\r\n}";

        public Func<UIHealthReport, bool> ShouldNotifyFunc { get; set; } = null;
        
        public Func<UIHealthReport, string> CustomMessageFunc { get; set; } = null;
        
        public Func<UIHealthReport, string> CustomDescriptionFunc { get; set; } = null;
    }

    public interface IWebhookSettings
    {
        string Name { get; }

        string Url { get; }

        string FailurePayload { get; }

        string RestorePayload { get; }

        Func<UIHealthReport, bool> ShouldNotifyFunc { get; }

        Func<UIHealthReport, string> CustomMessageFunc { get; }

        Func<UIHealthReport, string> CustomDescriptionFunc { get; }
    }
}
