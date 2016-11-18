using MonoServer.MonoContext;

namespace MonoServer.Components.Mvc
{
	public delegate void HttpAction(Context context);

	public interface IController
	{
	    void UseViewProvider(IViewProvider views);
		HttpAction Get { get; set; }
		HttpAction Post { get; set; }
		HttpAction Put { get; set; }
		HttpAction Delete { get; set; }
	}

	public static class IControllerExtensions
	{
		public static void Invoke(this IController controller, Context context)
		{
			switch (context.Request.Method)
			{
			case HttpMethod.Get:
				controller.Get?.Invoke(context);
			    break;
			case HttpMethod.Post:
				controller.Post?.Invoke (context);
			    break;
            case HttpMethod.Put:
                controller.Put?.Invoke(context);
                break;
            case HttpMethod.Delete:
                controller.Delete?.Invoke(context);
                break;
			}
		}
	}
}

