﻿using System.Text.Json;

namespace NotificationSender
{
    public class AlertNotification : Notification
    {
        public string Title { get; }
        public string Subtitle { get; }
        public string Body { get; }

        public AlertNotification(string title, string subtitle, string body, int? badge, string sound)
            : base(NotificationType.Alert, badge, sound)
        {
            Title = title;
            Subtitle = subtitle;
            Body = body;
        }

        public override string ToString()
        {
            var thisNotification = new
            {
                aps = new
                {
                    alert = new
                    {
                        title = Title,
                        subtitle = Subtitle,
                        body = Body,
                    },
                    badge = Badge,
                    sound = Sound
                }
            };

            return JsonSerializer.Serialize(thisNotification);
        }
    }
}