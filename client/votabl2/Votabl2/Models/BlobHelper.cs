using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using GalaSoft.MvvmLight.Messaging;

namespace Votabl2.Models
{
    public static class BlobHelper
    {
        public static async Task<string> UploadImageToBlobStorage(StorageFile image, string targetUrl)
        {
            Messenger.Default.Send<bool>(true, "busy");
            //Upload image with HttpClient to the blob service using the generated item.SAS
            using (var client = new HttpClient())
            {
                //Get a stream of the media just captured
                using (var fileStream = await image.OpenStreamForReadAsync())
                {
                    var content = new StreamContent(fileStream);
                    content.Headers.Add("Content-Type", image.ContentType);
                    content.Headers.Add("x-ms-blob-type", "BlockBlob");

                    using (var uploadResponse = await client.PutAsync(new Uri(targetUrl), content))
                    {
                        Messenger.Default.Send<bool>(false, "busy");
                        // remove the SAS querystring from the insert result
                        return targetUrl.Substring(0, targetUrl.IndexOf('?'));
                    }
                }
            }
        }
    }
}
