﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationSender
{
    public class NotificationSender : INotificationSender
    {
        private readonly string _host;

        private readonly ITokenGenerator _tokenGenerator;

        private static readonly HttpClient Client = new HttpClient();

        public NotificationSender(ITokenGenerator tokenGenerator, ApnsServerType serverType)
        {
            _tokenGenerator = tokenGenerator;

            var servers = new Dictionary<ApnsServerType, string>
            {
                {ApnsServerType.Development, "https://api.development.push.apple.com:443"},
                {ApnsServerType.Production, "https://api.push.apple.com:443"}
            };
            _host = servers[serverType];
        }

        public async Task<HttpResponseMessage> SendNotificationAsync(Notification notification,
            string deviceToken = "12345")
        {
            // var uri = new Uri($"{_host}/3/device/{deviceToken}");
            var uri = new Uri("https://localhost:5001/api/notifications/send-notification");

            var jwt = _tokenGenerator.GetValidToken();

            using var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Version = new Version(2, 0),
                Headers =
                {
                    {"authorization", "bearer " + jwt},
                    {"apns-push-type", notification.Type.ToString().ToLower()},
                    {"apns-priority", 10.ToString()},
                },
                Content = new StringContent(notification.ToString())
            };

            Console.WriteLine("request.Version: " + request.Version);

            return await Client.SendAsync(request);
        }
    }
}