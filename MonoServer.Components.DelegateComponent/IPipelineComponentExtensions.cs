using MonoServer.MonoContext;
using System;

namespace MonoServer.Components.Delegate
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent UseDelegate(this IPipelineComponent component, Action<Context, IPipelineComponent> action)
        {
            var newComponent = new DelegateComponent(component, action);
            component.Use(newComponent);
            return newComponent;                            
        }
    }
}
