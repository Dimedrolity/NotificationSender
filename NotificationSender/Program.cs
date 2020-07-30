﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationSender
{
    class Program
    {
        private static readonly ConcurrentQueue<AlertNotification> Queue = new ConcurrentQueue<AlertNotification>();

        public static async Task Main()
        {
            var keyId = "test-keyID";

            var privateSigningKey =
                @"MIGHAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBG0wawIBAQQgevZzL1gdAFr88hb2OF/2NxApJCzGCEDdfSp6VQO30hyhRANCAAQRWz+jn65BtOMvdyHKcvjBeBSDZH2r1RTwjmYSi9R/zpBnuQ4EiMnCqfMPWiZqB4QdbAd0E7oH50VpuZ1P087G";

            var teamId = "testTeamId";

            var tokenGenerator = new TokenGenerator(keyId, privateSigningKey, teamId);

            var notification = new AlertNotification("ttt", "sss", "bbb", 1, "sss");
            Enqueue(notification, 10_100);

            await SendNotificationsAsync(tokenGenerator);
        }

        private static void Enqueue(AlertNotification notification, int count)
        {
            for (var i = 0; i < count; i++)
            {
                Queue.Enqueue(notification);
            }
        }

        private static async Task SendNotificationsAsync(ITokenGenerator tokenGenerator)
        {
            var sender = new NotificationSender(tokenGenerator, ApnsServerType.Development);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (Queue.TryDequeue(out var notification))
            {
                var response = await sender.SendNotificationAsync(notification);
                Console.WriteLine("response.Version: " + response.Version);
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Ответ: " + body + "\n");
                Thread.Sleep(1000);
            }
            stopwatch.Stop();
            
            Console.WriteLine($"прошло {stopwatch.Elapsed.Minutes} мин, {stopwatch.Elapsed.Seconds} сек");
        }

        // private static void PrintToken(TokenGenerator tokenGenerator)
        // {
        //     for (var i = 1;; i += 10)
        //     {
        //         var token = tokenGenerator.GetValidToken();
        //         Console.WriteLine(token);
        //         Console.WriteLine($"прошло {i} сек.");
        //         Thread.Sleep(10000);
        //     }
        // }
    }
}