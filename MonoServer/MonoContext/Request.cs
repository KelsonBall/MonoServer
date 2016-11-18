using MonoServer.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MonoServer.MonoContext
{
    public enum HttpMethod
    {
        Get,
        Post,
        Patch,
        Put,
        Delete
    }

    public class Request
    {
        public HttpMethod Method { get; private set; }
        public IReadOnlyList<string> Parameters { get; private set; }
        public IReadOnlyDictionary<string, string> Query { get; private set; }
        public IReadOnlyDictionary<string, string> Headers { get; private set; }
        public IReadOnlyDictionary<string, Cookie> Cookies { get; private set; }
        private readonly string _requestUrl;
        public readonly bool HasContent;
        public byte[] ContentBytes { get; private set; }
        public string ContentText { get; private set; }
        public string ContentType { get; private set; }

        public Request(HttpListenerRequest request)
        {
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), request.HttpMethod, true);
            Query = ParseNameValueCollection(request.QueryString, s => HttpUtility.UrlDecode(s));
            Headers = ParseNameValueCollection(request.Headers);
            Cookies = ParseCookies(request.Cookies);
            _requestUrl = request.GetRequestUrl();
            HasContent = request.HasEntityBody;
            if (request.HasEntityBody)
            {
                ContentBytes = ParseInputStream(request.InputStream);
                ContentText = request.ContentEncoding.GetString(ContentBytes);
                ContentType = request.ContentType;
            }
        }

        private byte[] ParseInputStream(Stream inputStream)
        {            
            IList<byte> bytes = new List<byte>();
            int result;
            while ((result = inputStream.ReadByte()) >= 0)                       
                bytes.Add((byte)result);            
            return bytes.ToArray();
        }

        private IReadOnlyDictionary<string, string> ParseNameValueCollection(NameValueCollection collection, Func<string, string> decoder = null)
        {
            decoder = decoder ?? (s => s);
            var paramsDictionary = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                paramsDictionary.Add(key, decoder(collection[key]));
            }
            return new ReadOnlyDictionary<string, string>(paramsDictionary);
        }

        private IReadOnlyDictionary<string, Cookie> ParseCookies(CookieCollection cookies)
        {
            var paramsDictionary = new Dictionary<string, Cookie>();
            foreach (Cookie cookie in cookies)
            {
                paramsDictionary.Add(cookie.Name, cookie);
            }
            return new ReadOnlyDictionary<string, Cookie>(paramsDictionary);
        }

        internal void SetUrlParameters(string[] parameters)
        {
            Parameters = new List<string>(parameters).AsReadOnly();
        }

        public Cookie this[string key]
        {
            get
            {
                if (Cookies.ContainsKey(key))
                    return Cookies[key];
                return null;
            }
        }

        private string _requestUrlOverride;
        public string RequestUrl
        {
            get
            {
                if (_requestUrlOverride != null)
                    return _requestUrlOverride;
                string url = _requestUrl;
                while (url.Contains("\\") || url.Contains("//"))
                    url = url.Replace("\\", "").Replace("//", "/");
                return url;
            }
            set
            {
                _requestUrlOverride = value;
            }
        }
    }
}
