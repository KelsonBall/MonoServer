using System;
using MonoServer.MonoContext;

namespace MonoServer.Components.Mvc
{
	public delegate IExecutionModel HttpAction(Context context);

	public interface IController
	{
		HttpAction Get { get; set; }
		HttpAction Post { get; set; }
		HttpAction Put { get; set; }
		HttpAction Delete { get; set; }
	}

	public static class IControllerExtensions
	{
		public static IExecutionModel Invoke(this IController contoller, Context context, HttpMethod method)
		{
			switch (method)
			{
			case HttpMethod.Get:
				return controller.Get?.Invoke(context);
			case HttpMethod.Post:
				return controller.Post?.Invoke (context);
			}
		}
	}
}

