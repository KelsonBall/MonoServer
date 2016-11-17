using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
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

		private readonly IDictionary<string, string> _sourceMap;

		public LuaViewProvider(IDictionary<string, string> sourceMap)
		{
			_sourceMap = sourceMap;
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
		    script.AppendLine("function Each(collection) local e = collection:GetEnumerator() return function () if e:MoveNext() then return e.Current end end end");
			script.AppendLine ("local template = loadstring(templateModule)()");
			script.AppendLine ("local view = template.new(key, map)");
			foreach (var modelKey in model.Keys)
				script.AppendLine($"view.{modelKey} = model.{modelKey}");
			script.AppendLine ("return tostring(view)");
			return (string)state.DoString (script.ToString ()) [0];
		}
	}
}

