using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.Networking.PushNotifications;
using Votabl2.Models;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace Votabl2
{
    internal class votabl2Push
    {
        public async static void UploadChannel()
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            channel.PushNotificationReceived += channel_PushNotificationReceived;

            var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            string installationId = Windows.Security.Cryptography.CryptographicBuffer.EncodeToBase64String(token.Id);

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

        static async void channel_PushNotificationReceived(Windows.Networking.PushNotifications.PushNotificationChannel sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == PushNotificationType.Raw)
            {
                var json = JObject.Parse(args.RawNotification.Content);
                var vote = json.ToObject<RawVote>();
                await DispatcherHelper.RunAsync(() =>
                {
                    Messenger.Default.Send(vote);
                });
            }
        }

        private static void HandleInsertChannelException(Exception exception)
        {

        }
    }
}
