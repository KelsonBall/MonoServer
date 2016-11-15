﻿using MonoServer.DependencyInjection;
using System;

namespace MonoServer.Components
{
    public interface IPipelineRoot : IPipelineComponent
    {
        IContainer Injector { get; }
        IPipelineRoot AddDependencyConfigurationDelegate(Action<IContainer> configurer);
        void Start(string home, ushort port);
        void Stop();        
    }
}
