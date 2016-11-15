using System;

namespace MonoServer.Components
{    
    using MonoContext;

    public interface IPipelineComponent
    {        
        IPipelineComponent Parent { get; }
        IPipelineComponent Use(IPipelineComponent component);
        void Execute(Context context);
    }
}
