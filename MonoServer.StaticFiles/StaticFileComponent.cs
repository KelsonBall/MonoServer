using MonoServer.MonoContext;
using System.IO;

namespace MonoServer.Components.StaticFiles
{
    public class StaticFileComponent : IPipelineComponent
    {
        private readonly IPipelineComponent _parent;
        public IPipelineComponent Parent
        {
            get
            {
                return _parent;
            }
        }

        private IPipelineComponent next;
        public IPipelineComponent Use(IPipelineComponent component) => (next = component);

        private readonly string _contentDirectory;

        public StaticFileComponent(IPipelineComponent parent, string contentDirectory)
        {
            _parent = parent;
            _contentDirectory = new DirectoryInfo(contentDirectory).FullName;
        }

        public void Execute(Context context)
        {
            if (context.Authenticated)
            {
                string fileDir = _contentDirectory + Path.DirectorySeparatorChar + context.Request.RequestUrl;
                if (Directory.Exists(fileDir))
                {
                    fileDir = Path.Combine(fileDir, "index.html");
                }
                if (File.Exists(fileDir))
                {
                    FileInfo file = new FileInfo(fileDir);
                    context.Response.Write(file);
                    return;
                }
            }
            next?.Execute(context);
        }
    }
}
