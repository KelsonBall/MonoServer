using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MonoServer.DependencyInjection;
using MonoServer.MonoContext;

namespace MonoServer.Components.Mvc
{
    public class AssemblyControllerProvider : IControllerProvider
    {
        public IPipelineComponent Parent { get; }

        public void Execute(Context context)
        {
            var controller = Get(context.Request.RequestUrl());
            if (controller == null)
            {
                next.Execute(context);
                return;
            }
            controller.Invoke(context);
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);

        private readonly IDictionary<string, Func<IController>> _registrey = new Dictionary<string, Func<IController>>();

        private readonly IViewProvider _views;

        public AssemblyControllerProvider(IPipelineComponent parent, IViewProvider provider, params Assembly[] sourceAssemblies)
        {
            Parent = parent;
            _views = provider;
            IContainer diContainer = this.GetInjector();
            foreach (Assembly assembly in sourceAssemblies)
            {
                foreach (Type controllerType in assembly.GetTypes().Where(t => typeof(IController).IsAssignableFrom(t)))
                {
                    UrlAttribute urlOverride = controllerType.GetCustomAttribute<UrlAttribute>();
                    string url = urlOverride != null ? urlOverride.Url : controllerType.FullName.Replace('.', '/');
                    _registrey[url] = () => (IController)Activator.CreateInstance(controllerType);
                }
            }
        }

        public IController Get(string url)
        {
            if (url.StartsWith("/"))
                url = url.Substring(1);
            if (_registrey.ContainsKey(url))
            {
                IController controller = _registrey[url]();
                controller.UseViewProvider(_views);
                return controller;
            }
            return null;
        }

        public IControllerProvider RegisterController(string url, Func<IController> controllerFactory)
        {
            _registrey[url] = controllerFactory;
            return this;
        }
    }
}
