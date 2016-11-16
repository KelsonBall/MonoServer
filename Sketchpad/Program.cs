﻿using System;
using System.Collections.Generic;
using MonoServer.Components.Mvc;
using MonoServer.Components.Mvc.Views.Lua;

namespace Sketchpad
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Dictionary<string,string> map = {
				{ "index", "{(header)} <h1>{{message}}</h1> {(footer)}" },
				{ "header", "<html><head><title>Hello</title></head><body>" },
				{ "footer", "</body></html>" }
			};

			IViewProvider viewProvider = new LuaViewProvider (null, map);

			Dictionary<string, object> model = {
				{ "message", "Hello, world!" }
			};

			viewProvider.RenderView ("index", model);
		}
	}
}