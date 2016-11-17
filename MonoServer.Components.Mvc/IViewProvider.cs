using System;
using System.Collections.Generic;

namespace MonoServer.Components.Mvc
{
	public interface IViewProvider
	{		
		string RenderView(string key, IDictionary<string,object> model);
	}
}