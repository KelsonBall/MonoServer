using MonoServer.Components.Mvc;
using MonoServer.MonoContext;
using System.Collections.Generic;

namespace Project3.Controllers
{
    public class form : IController
    {
        public HttpAction Delete { get; set; }

        public HttpAction Get { get; set; }

        public HttpAction Post { get; set; }

        public HttpAction Put { get; set; }

        private IViewProvider _views;
        public void UseViewProvider(IViewProvider views)
        {
            _views = views;
        }

        public form()
        {
            Get += (Context context) =>
            {
                int next = 0;
                if (   context.Request.Query.ContainsKey ( "stateid" ) 
                    && context.Request.Query.ContainsKey ( "textfield1" )  
                    && context.Request.Query.ContainsKey ( "textfield2" ) 
                    && context.Request.Query.ContainsKey ( "textfield3" ) )
                {
                    State previous = Journey.GetState(int.Parse(context.Request.Query["stateid"]));
                    if (context.Request.Query["textfield1"].Equals(previous.Prompt1))
                        next = previous.Prompt1NextId;
                    else if (context.Request.Query["textfield2"].Equals(previous.Prompt2))
                        next = previous.Prompt2NextId;
                    else if (context.Request.Query["textfield3"].Equals(previous.Prompt3))
                        next = previous.Prompt3NextId;
                }
                Dictionary<string, object> model = new Dictionary<string, object>
                {
                    {"state", Journey.GetState(next) }
                };
                context.Response.Write(_views.RenderView("form.html", model));
            };

            Post += context =>
            {

            };
        }
    }
}
