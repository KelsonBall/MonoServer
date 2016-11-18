using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoServer.MonoContext;

namespace MonoServer.Components.Mvc
{
    public class AssemblyControllerProvider : IMvcPipelineComponent
    {
        public IPipelineComponent Parent { get; }

        public void Execute(Context context)
        {
            var controller = Get(context.Request.RequestUrl == "" || context.Request.RequestUrl == "/" ? _defaultRedirect : context.Request.RequestUrl);
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

        private IViewProvider _views;

        public AssemblyControllerProvider(IPipelineComponent parent, IViewProvider provider = null, string root = null, params Assembly[] sourceAssemblies)
        {
            Parent = parent;
            WithViews(provider);
            if (sourceAssemblies.Length > 0)
                WithControllers(root, sourceAssemblies);
        }

        public IMvcPipelineComponent WithControllers(string root, params Assembly[] sourceAssemblies)
        {
            foreach (Assembly assembly in sourceAssemblies)
            {
                foreach (Type controllerType in assembly.GetTypes()
                                                        .Where(t => t.FullName.StartsWith(root ?? ""))
                                                        .Where(t => typeof(IController).IsAssignableFrom(t)))
                {
                    UrlAttribute urlOverride = controllerType.GetCustomAttribute<UrlAttribute>();
                    string url = urlOverride != null ? urlOverride.Url : controllerType.FullName.Substring((root?.Length ?? -1) + 1).Replace('.', '/');
                    _registrey[url] = () => {
                        var controller = (IController)Activator.CreateInstance(controllerType);
                        controller.UseViewProvider(_views);
                        return controller;
                    };
                }
            }
            return this;
        }

        public IMvcPipelineComponent WithViews(IViewProvider views)
        {
            _views = views;
            return this;
        }

        private string _defaultRedirect = "";
        public IMvcPipelineComponent SetDefault(string url)
        {
            _defaultRedirect = url;
            return this;
        }

        public IController Get(string url)
        {
            if (url.StartsWith("/"))
                url = url.Substring(1);
            if (_registrey.ContainsKey(url))                            
                return _registrey[url]();            
            return null;
        }
    }
}
