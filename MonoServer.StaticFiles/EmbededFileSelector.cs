using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MonoServer.Components.StaticFiles
{
    public class EmbededFileSelector : Dictionary<string, Func<Stream>>
    {
        public EmbededFileSelector(IEnumerable<string> resources, Func<string, string> keySelector)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach (string resource in resources)
            {                
                this[keySelector(resource)] = () =>
                {
                    MemoryStream stream = new MemoryStream();
                    using (var resourceStream = assembly.GetManifestResourceStream(resource))
                        resourceStream.CopyToAsync(stream);
                    return stream;
                };
            }
        }
    }

    public class EmbededTextFileSelect : Dictionary<string, string>
    {
        public EmbededTextFileSelect(IEnumerable<string> resources, Func<string, string> keySelector)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach (string resource in resources)
            {
                string result;                
                using (var resourceStream = assembly.GetManifestResourceStream(resource))
                    using (var reader = new StreamReader(resourceStream))       
                        result = reader.ReadToEnd();
                this[keySelector(resource)] = result;
            }
        }
    }
}
