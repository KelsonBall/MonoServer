using System;
using MonoServer.MonoContext;

namespace MonoServer
{
	public interface IExecutionModel
	{
		void Execute(Context context);
	}
}

