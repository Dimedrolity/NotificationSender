﻿using NotificationSender;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class NotificationTests
    {
        [Test]
        public void ToString_AnyNotification_MatchesWithExpected()
        {
            var notification = new AlertNotification("tit", "subtit", "bbb", 4, "q.mp4");
            var actual = notification.ToString();
            var expected = GetExpectedToStringFrom(notification);

            Assert.AreEqual(expected, actual);
        }

        private string GetExpectedToStringFrom(AlertNotification notification)
        {
            return "{" +
                   "\"aps\":" + "{" +
                   "\"alert\":" + "{" +
                   $"\"title\":\"{notification.Title}\"," +
                   $"\"subtitle\":\"{notification.Subtitle}\"," +
                   $"\"body\":\"{notification.Body}\"" +
                   "}," +
                   $"\"badge\":{notification.Badge}," +
                   $"\"sound\":\"{notification.Sound}\"" +
                   "}}";
        }
    }
}