using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Dapper;

namespace MonoServer.Data.Crud.Sqlite
{
	public class SqliteDao : IDao
	{
		private Func<IDbConnection> _factory;

		private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();

		public SqliteDao ()
		{
		}

		#region IDao implementation

		public ICrudRepository<T> Get<T> () where T : IDbItem
		{
			return (ICrudRepository<T>)_repositories [typeof(T)];
		}

		public object Get(Type t)
		{
			return _repositories [t];
		}

		public IDao WithConnection (Func<IDbConnection> factory)
		{
			_factory = factory;
			return this;
		}

		public IDao WithModelsFrom (params System.Reflection.Assembly[] assemblies)
		{
			var tableGen = new SqliteTableGenerator ();
			foreach (var asm in assemblies)
			{
				foreach (var type in asm.GetTypes().Where(t => typeof(IDbItem).IsAssignableFrom(t)))
				{
					string createTableQuery = tableGen.GetCreateTableQuery (type);
					_factory.Do((c, t) => c.Execute(createTableQuery, transaction: t));
					Type repoType = typeof(AutoTypeRepository<>).MakeGenericType (type);
					_repositories[type] = Activator.CreateInstance(repoType, new [] { _factory });
				}
			}
			return this;
		}

		#endregion
	}
}

