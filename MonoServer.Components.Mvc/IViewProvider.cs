using System;
using System.Collections.Generic;

namespace MonoServer.Components.Mvc
{
	public interface IViewProvider : IPipelineComponent
	{
		IViewProvider UseRouter(Router router);
		string RenderView(string key, IDictionary<string,object> model);
	}
}