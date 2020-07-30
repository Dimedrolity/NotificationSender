﻿﻿﻿  namespace NotificationSender
{
    public abstract class Notification
    {
        public NotificationType Type { get; }

        public int? Badge { get; }

        public string Sound { get; }

        protected Notification(NotificationType type, int? badge, string sound)
        {
            Sound = sound;
            Badge = badge;
            Type = type;
        }
    }
}