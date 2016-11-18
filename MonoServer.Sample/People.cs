using System.Collections.Generic;
using MonoServer.Components.Mvc;

namespace Controllers.Api
{
    public class People : IController
    {
        private IViewProvider _views;
        public void UseViewProvider(IViewProvider views)
        {
            _views = views;
        }

        private List<string> _names = new List<string> { "Bob", "Sally", "Anne" };

        public People()
        {
            Get += context =>
                {
                    var model = new Dictionary<string, object> { { "people", _names  } };
                    string view = _views.RenderView("names.lua", model);
                    context.Response.Write(view);
                };

            Post += context =>
                {

                };
        }

        public HttpAction Get { get; set; }

        public HttpAction Post { get; set; }

        public HttpAction Put { get; set; }

        public HttpAction Delete { get; set; }
    }
}
