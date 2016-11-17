using System;

namespace MonoServer.Components
{    
    using MonoContext;

	public interface IPipelineComponent : IExecutionModel
    {        
        IPipelineComponent Parent { get; }
        IPipelineComponent Use(IPipelineComponent component);        
    }
}
