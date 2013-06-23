using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Models;

namespace Votabl2.Common
{
    public class ActivityHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Messenger.Default.Send<bool>(true, "busy");
            var response = await base.SendAsync(request, cancellationToken);
            Messenger.Default.Send<bool>(false, "busy");
            return response;
        }
    }
}
