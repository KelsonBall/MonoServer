using System.Reflection;

using MonoServer.Components.Delegate;
using MonoServer.Components.Mvc;
using MonoServer.Components.Mvc.Views.Lua;
using MonoServer.Components.Files;

namespace MonoServer.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            new HttpServer().UseDelegate((context, next) =>
                                {
                                    context.Authenticated = true;
                                    next?.Execute(context);
                                })
                            .UseLocalFiles("./web") 
                            .UseMvc()
                                .WithEmbededLuaViews("MonoServer.Sample.Views")
                                .WithControllers("Controllers")
                            .UseDelegate((context, next) =>
                                {
                                    next?.Execute(context);
                                    if (!context.Response.ResponseWritten)
                                        context.Response.Write("File Not Found");
                                })
                            .Start("localhost", 8080);            
        }
    }
}
