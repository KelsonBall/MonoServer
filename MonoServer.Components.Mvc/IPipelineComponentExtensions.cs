using System.Reflection;

namespace MonoServer.Components.Mvc
{
    public static class IPipelineComponentExtensions
    {
        public static IMvcPipelineComponent UseMvc(this IPipelineComponent component, IViewProvider views = null, string root = null, params Assembly[] assemblies)
        {
            var mvc = new AssemblyControllerProvider(component, views, root, assemblies);
            component.Use(mvc);
            return mvc;
        }
    }
}
