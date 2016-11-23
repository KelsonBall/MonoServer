using System;
using System.Data;
using System.Reflection;

namespace MonoServer.Data.Crud
{
	public interface IDao
	{
		ICrudRepository<T> Get<T>() where T : IDbItem;
		object Get (Type t);
		IDao WithConnection(Func<IDbConnection> factory);
		IDao WithModelsFrom(params Assembly[] assemblies);
	}
}