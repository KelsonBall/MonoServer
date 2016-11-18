using System.Reflection;

namespace MonoServer.Components.Mvc
{
    public interface IMvcPipelineComponent : IPipelineComponent
    {
        IMvcPipelineComponent SetDefault(string url);
        IMvcPipelineComponent WithViews(IViewProvider views);
        IMvcPipelineComponent WithControllers(string root , params Assembly[] sourceAssemblies);
        IController Get(string url);
    }
}
