using MonoServer.Components.Mvc;

namespace Controllers
{
    public class Form : IController
    {
        public HttpAction Delete { get; set; }

        public HttpAction Get { get; set; }

        public HttpAction Post { get; set; }

        public HttpAction Put { get; set; }

        private IViewProvider _views;
        public void UseViewProvider(IViewProvider views)
        {
            _views = views;
        }

        public Form()
        {
            Get += context =>
            {

            };

            Post += context =>
            {

            };
        }
    }
}
