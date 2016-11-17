using System;
using System.Collections.Generic;

namespace MonoServer.DependencyInjection
{
    enum ResourceType
    {
        Either = 0,
        Singleton = 1,
        Factory = 2
    }


    internal class Container : IContainer
    {
        private readonly Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public T Get<T>()
        {
            return Get<T>(ResourceType.Either);
        }

        public bool IsRegistered<T>()
        {
            var t = typeof(T);
            return factories.ContainsKey(t) || singletons.ContainsKey(t);
        }

        public T Get<T>(ResourceType type)
        {
            var t = typeof(T);
            switch (type)
            {
                case ResourceType.Singleton:
                    return (T)singletons[t];
                case ResourceType.Factory:
                    return (T)factories[t]();
                default:
                    if (singletons.ContainsKey(t))
                        return (T)singletons[t];
                    else
                        return (T)factories[t]();
            }
        }

        public IContainer RegisterFactory<Ti, T>(Func<T> factory) where T : Ti
        {
            factories[typeof(Ti)] = () => factory();
            return this;
        }

        public IContainer RegisterSingleton<Ti, T>(T item) where T : Ti
        {
            singletons[typeof(Ti)] = item;
            return this;
        }

        internal void Clear()
        {
            factories.Clear();
            singletons.Clear();
        }
    }
}
