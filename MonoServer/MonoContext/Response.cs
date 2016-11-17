using System;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace MonoServer.MonoContext
{
    public class Response
    {
        internal readonly HttpListenerResponse WrappedResponseObject;

        public int StatusCode
        {
            get { return WrappedResponseObject.StatusCode; }
            set { WrappedResponseObject.StatusCode = value; }
        }

        public bool ResponseWritten { get; internal set; } = false;

        public bool Authenticated { get; set; } = true;

        public string UnauthenticatedRedirect { get; set; }

        public string AuthenticatedRedirect { get; set; }

        public bool IsRedirected()
        {
            return !string.IsNullOrEmpty(UnauthenticatedRedirect) || !string.IsNullOrEmpty(AuthenticatedRedirect);
        }

        public Response(HttpListenerResponse response)
        {
            WrappedResponseObject = response;
        }

        public Cookie this[string key]
        {
            get
            {
                return WrappedResponseObject.Cookies[key];
            }
            set
            {
                value.Name = key;
                WrappedResponseObject.SetCookie(value);
            }
        }

        public void AddCookie(Cookie cookie)
        {
            WrappedResponseObject.Cookies.Add(cookie);
        }

        public void DeleteCookie(Cookie cookie)
        {
            cookie.Expires = DateTime.Now.AddDays(-1);
            WrappedResponseObject.Cookies.Add(cookie);
        }

        public bool Write(byte[] data)
        {
            if (ResponseWritten)
                return false;
            ResponseWritten = true;
            WrappedResponseObject.OutputStream.Write(data, 0, data.Length);
            return true;
        }

        public bool Write(string text, bool decode = false)
        {
            if (ResponseWritten)
                return false;
            ResponseWritten = true;
            if (decode)
                text = HttpUtility.HtmlDecode(text);
            byte[] data = WrappedResponseObject.ContentEncoding.GetBytes(text);
            WrappedResponseObject.OutputStream.Write(data, 0, data.Length);
            return true;
        }

        public bool Write(FileInfo file, string contentType = "text/html")
        {
            if (ResponseWritten)
                return false;
            if (!file.Exists)
            {
                StatusCode = 404;
                return false;
            }
            
            WrappedResponseObject.ContentType = contentType;            
            ResponseWritten = true;
            using (FileStream stream = new FileStream(file.FullName, FileMode.Open))
            {
                stream.CopyTo(WrappedResponseObject.OutputStream);
            }
            return true;
        }

        public bool Write(Stream stream, string content = null)
        {
            if (ResponseWritten)
                return false;
            if (content != null)
                WrappedResponseObject.ContentType = content;
            ResponseWritten = true;
            stream.CopyTo(WrappedResponseObject.OutputStream);
            return true;
        }

        public bool WriteJson<T>(T item)
        {
            if (ResponseWritten)
                return false;
            ResponseWritten = true;
            string json = JsonConvert.SerializeObject(item);
            byte[] data = WrappedResponseObject.ContentEncoding.GetBytes(json);
            WrappedResponseObject.OutputStream.Write(data, 0, data.Length);
            return true;
        }
    }
}
