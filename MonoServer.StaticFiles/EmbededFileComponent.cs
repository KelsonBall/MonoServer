using MonoServer.MonoContext;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoServer.Components.StaticFiles
{
    public class EmbededFileComponent : IPipelineComponent
    {
        private readonly IPipelineComponent _parent;
        public IPipelineComponent Parent
        {
            get
            {
                return _parent;
            }
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);
        
        private readonly IDictionary<string, Func<MemoryStream>> _content;
        public EmbededFileComponent(IPipelineComponent parent, IDictionary<string, Func<MemoryStream>> files)
        {
            _content = files;
        }

        public void Execute(Context context)
        {
            if (context.Authenticated)
            {
                string url = context.Request.RequestUrl();
                if (_content.ContainsKey(url))
                {
                    context.Response.Write(_content[url]());
                }
            }
            next?.Execute(context);
        }

    }
}
