using System;
using MonoServer.MonoContext;

namespace MonoServer.Components.Mvc
{
	public class MvcComponent : IPipelineComponent
	{
		public IPipelineComponent Parent { get; }

		public void Execute(Context context)
		{
		}

		private IPipelineComponent next;
		public IPipelineComponent Use(IPipelineComponent component) => (next = component);

		private readonly IViewProvider _viewProvider;
		public MvcComponent (IPipelineComponent parent, IControllerProvider controllerProvider, IViewProvider viewProvider)
		{
			_viewProvider = viewProvider;
		}
	}
}