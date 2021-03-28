using System;
using HealthChecks.UI.Configuration;

namespace WebApi.Example14
{
    public static class Extensions
    {
        public static Settings AddWebhookNotifications(this Settings settings, params IWebhookSettings[] webhooks)
        {
            foreach (var webhook in webhooks ?? Array.Empty<IWebhookSettings>())
            {
                settings.AddWebhookNotification(webhook.Name, webhook.Url, webhook.FailurePayload,
                    webhook.RestorePayload, webhook.ShouldNotifyFunc, webhook.CustomMessageFunc,
                    webhook.CustomDescriptionFunc);
            }

            return settings;
        }
    }
}
