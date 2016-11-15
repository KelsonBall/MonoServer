using System;
using MonoServer.Components;
using MonoServer.Components.Delegate;
using MonoServer.Components.StaticFiles;

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
                            .UseStaticFiles("./web")
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
