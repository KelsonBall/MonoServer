namespace MonoServer.Components.HelloWorld
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent UseHelloWorld(this IPipelineComponent component)
        {
            var helloworld = new HelloWorldComponent(component);
            component.Use(helloworld);
            return helloworld;
        }
    }
}
