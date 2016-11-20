using System.Reflection;

using MonoServer;
using MonoServer.Components.Delegate;
using MonoServer.Components.StaticFiles;
using MonoServer.Components.Mvc;
using MonoServer.Components.Mvc.Views;
using MonoServer.Components.Mvc.Views.Lua;
using System;

namespace Project3
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly local = Assembly.GetExecutingAssembly();
            new HttpServer().UseEmbededFiles("Project3.Web", local)
                            .UseMvc()
                                .WithViews(new LuaViewProvider(new EmbededResourceMap("Project3.Views", local)))
                                .WithControllers("Project3.Controllers", local)
                                .SetDefault("form")
                            .UseDelegate((context, next) => { if (!context.Response.ResponseWritten) context.Response.Write("Request reached end of response pipeline"); })
#if DEBUG
                            .Start("localhost", 8080);
#else
                            .Start("kelsonball.net", 80);
#endif            
        }
    }
}
