using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushnotificationsDemo.Models
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationoptions">Notification API Standard</see>
    /// </summary>
    public class Notification
    {
        public Notification() { }

        public Notification(string text)
        {
            Body = text;
        }

        [JsonProperty("title")]
        public string Title { get; set; } = "Push Demo";

        [JsonProperty("lang")]
        public string Lang { get; set; } = "en";

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonProperty("requireInteraction")]
        public bool RequireInteraction { get; set; } = false;

        [JsonProperty("actions")]
        public List<NotificationAction> Actions { get; set; } = new List<NotificationAction>();
    }

    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationaction">Notification API Standard</see>
    /// </summary>
    public class NotificationAction
    {

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class NotificationTag
    {
        public const string Notify = "demo_testmessage";
        public const string Trivia = "demo_trivia";
    }
}
