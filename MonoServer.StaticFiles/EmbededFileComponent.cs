using MonoServer.MonoContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MonoServer.Components.StaticFiles
{
    public class EmbededFileComponent : IPipelineComponent
    {        
        public IPipelineComponent Parent { get; }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);
        
        private readonly IDictionary<string, Func<Stream>> _content;
        public EmbededFileComponent(IPipelineComponent parent, string root, params Assembly[] assemblies)
        {
            Parent = parent;
            List<Tuple<string, Assembly>> resources = new List<Tuple<string, Assembly>>();
            foreach (Assembly asm in assemblies)
                foreach (string resource in asm.GetManifestResourceNames())
                    if (resource.StartsWith(root))
                        resources.Add(new Tuple<string, Assembly>(resource, asm));

            foreach (var resource in resources)                
                _content[resource.Item1.Substring(root.Length + 1)] = () => resource.Item2.GetManifestResourceStream(resource.Item1);
        }

        public void Execute(Context context)
        {
            if (context.Authenticated)
            {
                string url = context.Request.RequestUrl;
                if (_content.ContainsKey(url))
                {
                    context.Response.Write(_content[url]());
                }
            }
            next?.Execute(context);
        }

    }
}
