using Microsoft.Practices.Unity;
using System;

namespace MonoServer.Components
{
    public interface IPipelineRoot : IPipelineComponent
    {
        IUnityContainer Container { get; }
        IPipelineRoot ConfigureContainer(Action<IUnityContainer> configurer);
        void Start(string home, ushort port);
        void Stop();        
    }
}
