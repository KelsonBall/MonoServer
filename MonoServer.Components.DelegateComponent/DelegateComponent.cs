using System;
using MonoServer.MonoContext;

namespace MonoServer.Components.Delegate
{
    public class DelegateComponent : IPipelineComponent
    {        
        public IPipelineComponent Parent { get; }

        private readonly Action<Context, IPipelineComponent> _function;
        public void Execute(Context context)
        {
            _function?.Invoke(context, next);
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);

        public DelegateComponent(IPipelineComponent parent, Action<Context, IPipelineComponent> function)
        {
            Parent = parent;
            _function = function;
        }
    }
}
