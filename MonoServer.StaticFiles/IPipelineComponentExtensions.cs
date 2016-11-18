
using System.Reflection;

namespace MonoServer.Components.StaticFiles
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent UseStaticFiles(this IPipelineComponent component, string path)
        {
            var fileComponent = new StaticFileComponent(component, path);
            component.Use(fileComponent);
            return fileComponent;
        }

        public static IPipelineComponent UseEmbededFiles(this IPipelineComponent component, string path, params Assembly[] assemblies)
        {
            var fileComponent = new EmbededFileComponent(component, path, assemblies);
            component.Use(fileComponent);
            return fileComponent;
        }
    }
}
