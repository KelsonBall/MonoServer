using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using MonoServer.MonoContext;
using NLua;

namespace MonoServer.Components.Mvc.Views.Lua
{
	public class LuaViewProvider : IViewProvider
	{
		static string template;

		static LuaViewProvider()
		{			
			using (var stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("MonoServer.Components.Mvc.Views.Lua.template.lua"))
				using (var reader = new StreamReader (stream))
					template = reader.ReadToEnd ();
		}

		public IPipelineComponent Parent { get; }


		public void Execute(Context context)
		{			
		}

		private IPipelineComponent next;
		public IPipelineComponent Use(IPipelineComponent component) => (next = component);

		private readonly IDictionary<string, string> _sourceMap;

		public LuaViewProvider(IPipelineComponent parent, IDictionary<string, string> sourceMap)
		{
			Parent = parent;
			_sourceMap = sourceMap;
		}

		public string RenderView(string key, IDictionary<string,object> model)
		{
			Lua state = new Lua ();
			state.LoadCLRPackage ();
			state.AddTable ("model");
			state ["templateModule"] = template;
			state ["key"] = key;
			state ["map"] = _sourceMap;
			StringBuilder script = new StringBuilder ();
			script.AppendLine ("local template = load(templateModule)()");
			script.AppendLine ("local view = template.new(key, map)");
		}
	}
}

