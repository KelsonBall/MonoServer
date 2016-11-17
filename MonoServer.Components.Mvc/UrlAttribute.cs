using System;

namespace MonoServer.Components.Mvc
{
    public class UrlAttribute : Attribute
    {
        public readonly string Url;

        public UrlAttribute(string url)
        {
            Url = url;
        }
    }
}
