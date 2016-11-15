
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
    }
}
