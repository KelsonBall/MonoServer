using System;

namespace MonoServer.Components.Mvc
{
	public interface IControllerProvider : IPipelineComponent
	{
		IController Get(string url);
		IControllerProvider RegisterController (string url, Func<IController> controllerFactory);
	}
}

