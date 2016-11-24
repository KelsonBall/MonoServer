
using System.Reflection;

namespace MonoServer.Components.Files
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent UseLocalFiles(this IPipelineComponent component, string path)
        {
            var fileComponent = new LocalFileComponent(component, path);
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
