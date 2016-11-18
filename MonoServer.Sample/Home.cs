using System.Collections.Generic;
using MonoServer.Components.Mvc;
using MonoServer.MonoContext;

namespace Controllers
{
    public class Home : IController
    {
        private IViewProvider _views;
        public void UseViewProvider(IViewProvider views)
        {
            _views = views;
        }

        public Home()
        {
            Get += GetIndex;
        }

        private void GetIndex(Context context)
        {
            string view = _views.RenderView("index.lua", new Dictionary<string, object> { { "message", "Hello, World!" } });
            context.Response.Write(view);
        }

        public HttpAction Get { get; set; }

        public HttpAction Post { get; set; }

        public HttpAction Put { get; set; }

        public HttpAction Delete { get; set; }
    }
}

