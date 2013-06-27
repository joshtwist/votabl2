using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;
using Windows.System.Profile;
using GalaSoft.MvvmLight.Threading;
using Votabl2.Models;
using GalaSoft.MvvmLight.Messaging;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace Votabl2
{
    internal class votabl2Push
    {
        public async static void UploadChannel()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var token = HardwareIdentification.GetPackageSpecificToken(null);
            string installationId = CryptographicBuffer.EncodeToBase64String(token.Id);

            var ch = new JObject();
            ch.Add("channelUri", channel.Uri);
            ch.Add("installationId", installationId);

            channel.PushNotificationReceived += channel_PushNotificationReceived;

            try
            {
                await App.votabl2Client.GetTable("channels").InsertAsync(ch);
            }
            catch (Exception exception)
            {
                HandleInsertChannelException(exception);
            }
        }

        static void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == PushNotificationType.Raw)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var vote = JObject.Parse(args.RawNotification.Content).ToObject<RawVote>();
                    Messenger.Default.Send<RawVote>(vote);
                });

            }
        }

        private static void HandleInsertChannelException(Exception exception)
        {

        }
    }
}
