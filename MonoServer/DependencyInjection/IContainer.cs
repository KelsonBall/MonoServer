using System;

namespace MonoServer.DependencyInjection
{

    public interface IContainer
    {
        IContainer RegisterFactory<Ti, T>(Func<T> factory) where T : Ti;
        IContainer RegisterSingleton<Ti, T>(T item) where T : Ti;
        T Get<T>();
    }
}
