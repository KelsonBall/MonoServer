﻿using MonoServer.Components;
using System;

namespace MonoServer
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineRoot Start(this IPipelineComponent component, string home, ushort port)
        {
            var root = component.GetRoot();
            root.Start(home, port);
            return root;
        }			

        public static IPipelineRoot GetRoot(this IPipelineComponent component)
        {
            while (!(component is IPipelineRoot) && component != null)
                component = component.Parent;
            if (component == null)
                throw new ArgumentException("Component does not have an IPipelineRoot ancestor");
            return (IPipelineRoot)component;
        }
    }
}
