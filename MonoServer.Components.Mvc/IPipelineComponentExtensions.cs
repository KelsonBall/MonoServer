using System.Reflection;

namespace MonoServer.Components.Mvc
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent UseMvc(this IPipelineComponent component, IViewProvider views, params Assembly[] assemblies)
        {
            var mvc = new AssemblyControllerProvider(component, views, assemblies);
            component.Use(mvc);
            return mvc;
        }
    }
}
