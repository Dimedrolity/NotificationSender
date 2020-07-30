﻿using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationSender
{
    public interface INotificationSender
    {
        public Task<HttpResponseMessage> SendNotificationAsync(Notification notification, string deviceToken);
    }
}