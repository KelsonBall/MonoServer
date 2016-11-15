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

        public T Get<T>(ResourceType type)
        {
            switch (type)
            {                                    
                case ResourceType.Singleton:
                    return (T)singletons[typeof(T)];
                case ResourceType.Factory:
                    return (T)factories[typeof(T)]();
                default:
                    if (singletons.ContainsKey(typeof(T)))
                        return (T)singletons[typeof(T)];
                    else
                        return (T)factories[typeof(T)]();
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
