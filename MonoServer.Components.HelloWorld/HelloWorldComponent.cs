using MonoServer.MonoContext;

namespace MonoServer.Components.HelloWorld
{
    internal class HelloWorldComponent : IPipelineComponent
    {

        private readonly IPipelineComponent _parent;
        public IPipelineComponent Parent
        {
            get
            {
                return _parent;
            }
        }

        public void Execute(Context context)
        {
            next?.Execute(context);
            if (!context.Response.ResponseWritten)
                context.Response.Write("Hello, World!");
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);

        public HelloWorldComponent(IPipelineComponent parent)
        {
            _parent = parent;
        }
    }
}
