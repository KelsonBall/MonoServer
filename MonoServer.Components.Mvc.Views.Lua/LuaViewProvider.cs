﻿using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using MonoServer.MonoContext;
using NLua;
using LuaScriptEngine = NLua.Lua;
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

		private Router _router;
		public IViewProvider UseRouter(Router router)
		{
			_router = router;
			return this;
		}

		public string RenderView(string key, IDictionary<string,object> model)
		{
			LuaScriptEngine state = new LuaScriptEngine ();
			state.LoadCLRPackage ();
			state ["templateModule"] = template;
			state ["key"] = key;
			state ["map"] = _sourceMap;
			state ["model"] = model;
			StringBuilder script = new StringBuilder ();
			script.AppendLine ("local template = loadstring(templateModule)()");
			script.AppendLine ("local view = template.new(key, map)");
			foreach (var modelKey in model.Keys)
				script.AppendLine($"view.{modelKey} = model[{modelKey}]");
			script.AppendLine ("return tostring(view)");
			return (string)state.DoString (script.ToString ()) [0];
		}

        public IViewProvider UseRouter(Router router)
        {
            throw new NotImplementedException();
        }
    }
}

