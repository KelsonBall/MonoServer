using System;

namespace MonoServer.Components.Mvc
{
	public interface IControllerProvider
	{
		IController Get(string url);
		IControllerProvider RegisterController (string urlPattern, Func<IController> controllerFactory);
	}
}

