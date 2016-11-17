using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MonoServer.Components.Mvc.Views
{
    public class EmbededResourceMap : Dictionary<string, string>
    {
        public EmbededResourceMap(string root = "", params Assembly[] assemblies)
        {
            List<Tuple<string, Assembly>> resources = new List<Tuple<string, Assembly>>();
            foreach (Assembly asm in assemblies)
                foreach (string resource in asm.GetManifestResourceNames())
                    if (resource.StartsWith(root))
                        resources.Add(new Tuple<string, Assembly>(resource, asm));

            foreach (var resource in resources)
                using (var stream = resource.Item2.GetManifestResourceStream(resource.Item1))
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                            this[resource.Item1.Substring(root.Length + 1)] = reader.ReadToEnd();
        }

        public EmbededResourceMap Use(string root, Assembly asm)
        {
            List<string> resources = new List<string>();
            foreach (string resource in asm.GetManifestResourceNames())
                if (resource.StartsWith(root))
                    resources.Add(resource);

            foreach (var resource in resources)
                using (var stream = asm.GetManifestResourceStream(resource))
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                            this[resource.Substring(root.Length)] = reader.ReadToEnd();

            return this;
        }
    }
}
