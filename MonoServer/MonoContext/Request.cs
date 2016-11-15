using MonoServer.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
            Query = ParseNameValueCollection(request.QueryString);
            Headers = ParseNameValueCollection(request.Headers);
            Cookies = ParseCookies(request.Cookies);
            _requestUrl = request.GetRequestUrl();
            HasContent = request.HasEntityBody;
            if (request.HasEntityBody)
            {
                ContentBytes = ParseInputStream(request.InputStream);
                ContentText = HttpUtility.HtmlEncode(request.ContentEncoding.GetString(ContentBytes));
                ContentType = request.ContentType;
            }
        }

        private byte[] ParseInputStream(Stream inputStream)
        {
            byte[] data = new byte[inputStream.Length];
            int read;
            int offset = 0;
            while ((read = inputStream.Read(data, offset, data.Length - offset)) > 0)
                offset += read;
            return data;
        }

        private IReadOnlyDictionary<string, string> ParseNameValueCollection(NameValueCollection collection)
        {
            var paramsDictionary = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                paramsDictionary.Add(key, collection[key]);
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

        public string RequestUrl(string path = null)
        {
            return path + Path.DirectorySeparatorChar + _requestUrl;
        } 
    }
}
