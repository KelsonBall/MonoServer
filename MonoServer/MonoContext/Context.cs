
using System.Net;

namespace MonoServer.MonoContext
{
    public class Context
    {
        public bool Authenticated = false;
        public readonly HttpListenerContext InternalContext;
        public readonly Request Request;
        public readonly Response Response;

        public Context(HttpListenerContext context)
        {
            InternalContext = context;
            Request = new Request(context.Request);
            Response = new Response(context.Response);
        }
    }
}
