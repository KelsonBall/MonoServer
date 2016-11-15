using System;
using System.Runtime.CompilerServices;

namespace MonoServer.Logging
{
    public enum Serverity
    {
        Low,
        Medium,
        High
    }

    public interface ILogger
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        Action Log(string name, string message, Serverity serverity);
    }
}
