using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonoServer.Extensions
{
    public static class HttpListenerRequestExtensions
    {
        public static string GetRequestUrl(this HttpListenerRequest request)
        {
            string url = request.Url.AbsolutePath;
            string referrer = request.RawUrl.StartsWith("/") ? string.Empty : request.UrlReferrer.AbsolutePath;

            return $@"{referrer}/{url}";
        }
    }
}
