using System.Reflection;

namespace MonoServer.Components.Mvc.Views.Lua
{
    public static class IMvcPipelineComponentExtensions
    {
        public static IMvcPipelineComponent WithEmbededLuaViews(this IMvcPipelineComponent component, string root, params Assembly[] assemblies)
        {
            component.WithViews(new LuaViewProvider(new EmbededResourceMap(root, assemblies)));
            return component;
        }
    }
}
