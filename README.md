# MonoServer - Web Backend Framework

MonoServer is a backend framework for developing server code in C#. 
It is architecturally inspired by asp.net for dotnet core. What makes MonoServer unique
and useful is its deployment methodology, which is summed up as 'Put your files on your server and run the program.'

MonoServer applications are executable programs. They are self hosted and are started by 
running the executable and stopped by closing the process. 

## In The Works:

 * Docker Container and Digital Ocean images for running MonoServer applications using an Nginx reverse proxy.
 * Front end libraries deployed via nuget - project goal is to avoid dependency on npm / node.
 * `Response.Write` decorator pattern implementation.
 * Getting started wiki
 * `Context` interface segregation and `System.Web` independent implementation to facilitate testing

## Sample:

See the sample project at https://github.com/KelsonBall/MonoServer/tree/master/MonoServer.Sample

The entry point for a MonoServer application is the Main method (!), found in Program.cs for the sample. 

## Core Concept

MonoServer serves requests by implementing a request pipeline. The pipeline starts with an `HttpServer` instance which passes requests to user defined `IPipeLineComponent` implementations. 

This example pipes requests to a delegate that whitelists all requests and then serves files from the `web` directory on disk.

    // Import the components from their packages
    using MonoServer;
    using MonoServer.Components.Delegate;
    using MonoServer.Components.Files;

    public class Program
    {
        public static void Main()
        {
            // create an HttpServer
            new HttpServer()
                            // Pipe requests to this delegate
                            // context is the object representing the web request
                            // next is the next IPipeLineComponent in the request pipeline
                            .UseDelegate((context, next) =>
                                {
                                    context.Authenticated = true;
                                    next?.Execute(context);
                                })
                            // After the delegate add a LocalFiles component
                            .UseLocalFiles("./web")
                            // Start the server on localhost at port 8080
                            .Start("localhost", 8080);
        } 
    }

