using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace Votabl2
{
    internal class votabl2Push
    {
        public async static void UploadChannel()
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            string installationId = Windows.Security.Cryptography.CryptographicBuffer.EncodeToBase64String(token.Id);

            channel.PushNotificationReceived += channel_PushNotificationReceived;

            var ch = new JObject();
            ch.Add("channelUri", channel.Uri);
            ch.Add("installationId", installationId);

            try
            {
                await App.votabl2Client.GetTable("channels").InsertAsync(ch);
            }
            catch (Exception exception)
            {
                HandleInsertChannelException(exception);
            }
        }

        static void channel_PushNotificationReceived(Windows.Networking.PushNotifications.PushNotificationChannel sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == Windows.Networking.PushNotifications.PushNotificationType.Raw)
            {
                NotificationArrived(args.RawNotification.Content);
            }
        }

        private static void HandleInsertChannelException(Exception exception)
        {

        }

        public static Action<string> NotificationArrived;
    }
}
