using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using GalaSoft.MvvmLight.Messaging;
using Votabl2.Common;
using System.Net;

namespace Votabl2.Common
{
    public static class BlobHelper
    {
        public static async Task<string> UploadImageToBlobStorage(StorageFile image, string targetUrl)
        {
            //Upload image with HttpClient to the blob service using the generated item.SAS

            // TODO - add BusyDisposer
            using (new BusyDisposer())
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
                        if (uploadResponse.StatusCode != HttpStatusCode.Created)
                        {
                            throw new Exception(string.Format("Upload Failed {0}", uploadResponse.ToString()));
                        }
                        // remove the SAS querystring from the insert result
                        return targetUrl.Substring(0, targetUrl.IndexOf('?'));
                    }
                }
            }
        }
    }
}
