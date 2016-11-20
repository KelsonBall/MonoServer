using MonoServer.Components;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using MonoServer.MonoContext;
using System.Net;

namespace MonoServer
{
    public class HttpServer : IPipelineRoot
    {
        private readonly IUnityContainer _container;
        public IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public IPipelineComponent Parent
        {
            get
            {
                return null;
            }
        }

        public HttpServer()
        {
            _container = new UnityContainer();
        }

        private Action<IUnityContainer> configurationDelegate;
        public IPipelineRoot ConfigureContainer(Action<IUnityContainer> configurer)
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
            configurationDelegate?.Invoke(_container);
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
                        catch (Exception e)
                        {
                            throw e;
                        }
                        finally
                        {
                            context?.Response.OutputStream.Flush();
                            context?.Response.OutputStream.Close();
                        }
                    });
                }
            });
            string message;
            while (!(message = Console.ReadLine()).Equals("exit")) ;
        }

        public void Stop()
        {
            Running = false;
        }
    }
}
