using System;
using System.Reflection;

using MonoServer.Components.Delegate;
using MonoServer.Components.Mvc;
using MonoServer.Components.Mvc.Views;
using MonoServer.Components.Mvc.Views.Lua;
using MonoServer.Components.StaticFiles;

namespace MonoServer.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly local = Assembly.GetExecutingAssembly();
            new HttpServer().UseDelegate((context, next) =>
                                {
                                    context.Authenticated = true;
                                    next?.Execute(context);
                                })
                            .UseStaticFiles("./web")
                            .UseMvc(new LuaViewProvider(new EmbededResourceMap("MonoServer.Sample.Views", local)), "Controllers", local)
                            .UseDelegate((context, next) =>
                                {
                                    next?.Execute(context);
                                    if (!context.Response.ResponseWritten)
                                        context.Response.Write("File Not Found");
                                })
                            .Start("localhost", 8080);
            Console.ReadKey();
        }
    }
}
