using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MonoServer.Data.Crud
{
	public interface ICrudRepository<T> where T : IDbItem
	{
		ICrudRepository<T> Create (T model);
		ICrudRepository<T> Create(IEnumerable<T> models);

		IEnumerable<T> Read ();

		ICrudRepository<T> Update (T model);
		ICrudRepository<T> Update (IEnumerable<T> models);

		ICrudRepository<T> Delete (T model);
		ICrudRepository<T> Delete (IEnumerable<T> models);
	}
}

