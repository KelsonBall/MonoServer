using MonoServer.Components;
using System;
using System.Text;
using System.Threading.Tasks;
using MonoServer.DependencyInjection;
using MonoServer.MonoContext;
using System.Net;

namespace MonoServer
{
    public class HttpServer : IPipelineComponent, IPipelineRoot
    {
        private IContainer _injector;
        public IContainer Injector
        {
            get
            {
                return _injector;
            }
        }

        public IPipelineComponent Parent
        {
            get
            {
                return null;
            }
        }

        private Action<IContainer> configurationDelegate;
        public IPipelineRoot AddDependencyConfigurationDelegate(Action<IContainer> configurer)
        {
            configurationDelegate += configurer;
            return this;
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component)
        {
            next = component;
            return this;
        }

        public void Execute(Context context)
        {
            next.Execute(context);
        }

        private bool Running = false;
        public void Start(string home, ushort port)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($@"http://{home}:{port}/");            
            listener.Start();
            Running = true;
            Task.Factory.StartNew(() =>
            {
                while (Running)
                {
                    HttpListenerContext context = listener.GetContext();
                    context.Response.ContentEncoding = context.Response.ContentEncoding ?? Encoding.UTF8;
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Context args = new Context(context);
                            Execute(args);                            
                        }
                        finally
                        {
                            context?.Response.OutputStream.Flush();
                            context?.Response.OutputStream.Close();
                        }
                    });
                }
            });
        }

        public void Stop()
        {
            Running = false;
        }        
    }
}
