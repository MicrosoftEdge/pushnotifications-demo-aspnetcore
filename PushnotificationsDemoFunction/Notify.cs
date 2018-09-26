using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PushnotificationsDemo.Models;
using PushnotificationsDemo.Services;
using System;
using System.Threading.Tasks;

namespace PushnotificationsDemoFunction
{
    public static class Notify
    {
        [FunctionName("Notify")]
        public static async Task RunAsync([TimerTrigger("0 0 7 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Notify function executed at: {DateTime.Now}");

            var vapidSubject = Environment.GetEnvironmentVariable("VapidSubject", EnvironmentVariableTarget.Process);
            var vapidPublicKey = Environment.GetEnvironmentVariable("VapidPublicKey", EnvironmentVariableTarget.Process);
            var vapidPrivateKey = Environment.GetEnvironmentVariable("VapidPrivateKey", EnvironmentVariableTarget.Process);

            // Get subscriptions from SQL database
            var context = new DemoDbContext();
            var subscriptions = await context.PushSubscription.ToListAsync();

            if (subscriptions.Count == 0)
            {
                log.LogInformation("No subscriptions were found in the database. Exciting.");
                return;
            }

            log.LogInformation($"{subscriptions.Count} subscriptions was found in the database.");

            var pushService = new PushService(context, vapidSubject, vapidPublicKey, vapidPrivateKey);

            var pushMessage = TriviaList[DateTime.Today.DayOfYear % TriviaList.Length];

            log.LogInformation($"Trivia of the day: {pushMessage}");

            var notification = new Notification
            {
                Title = "Did you know?",
                Body = pushMessage,
                Tag = NotificationTag.Trivia,
            };

            foreach (var subscription in subscriptions)
            {
                try { await pushService.Send(subscription.UserId, notification); }
                catch (Exception e)
                {
                    throw new Exception($"Failed to send push to user with id: {subscription.UserId}. See inner exception.", e);
                }
            }

            log.LogInformation("All push notifications were sent successfully. Exciting.");
        }

        internal static readonly string[] TriviaList =
        {
            "Astrology is the study of celestial objects and their position's affect on humans and earth",
            "The zodiac is divided into twelve sections with signs that form the celestial sphere",
            "Aries is the first astrological sign of the zodiac",
            "Aries' symbol is a ram",
            "Taurus is the second astrological sign of the zodiac",
            "Taurus' symbol is a bull",
            "Gemini is the third astrological sign of the zodiac",
            "Gemini's symbol is twins",
            "Cancer is the fourth astrological sign of the zodiac",
            "Cancer's symbol is a crab",
            "Leo is the fifth astrological sign of the zodiac",
            "Leo's symbol is a lion",
            "Virgo is the sixth astrological sign of the zodiac",
            "Virgo's symbol is a virgin",
            "Libra is the seventh astrological sign of the zodiac",
            "Libra's symbol is scales",
            "Scorpio is the eighth astrological sign of the zodiac",
            "Scorpio's symbol is a scorpion",
            "Sagittarius is the ninth astrological sign of the zodiac",
            "Sagittarius' symbol is an archer",
            "Capricorn is the tenth astrological sign of the zodiac",
            "Capricorn's symbol is a goat-fish hybrid",
            "Aquarius is the eleventh astrological sign of the zodiac",
            "Aquarius' symbol is a water-bearer",
            "Pisces is the twelfth astrological sign of the zodiac",
            "Pisces' symbol is a fish"
        };
    }
}
