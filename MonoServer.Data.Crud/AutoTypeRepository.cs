using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MonoServer.Data.Crud
{
	public class AutoTypeRepository<T> : ICrudRepository<T> where T : IDbItem
	{
		private readonly Func<IDbConnection> _connectionFactory;
		private readonly string _createCommand;
		private readonly string _readCommand;
		private readonly string _updateCommand;
		private readonly string _deleteCommand;

		public AutoTypeRepository (Func<IDbConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory;

			Type t = typeof(T);
			var parameterMembers = t.GetProperties (BindingFlags.Public | BindingFlags.Instance).Where(p => !p.Name.Equals("Id"));
			Func<IEnumerable<string>, string> combine = list => string.Join (", ", list);
			string properties_get = combine(parameterMembers.Select (p => p.Name));
			string properties_set = combine (parameterMembers.Select (p => "@" + p.Name));
			string properties_update = combine(parameterMembers.Select(p => p.Name + " = @" + p.Name));
			_createCommand = "insert into " + t.Name + "sTable"  + " ( Id, " + properties_get + " ) values ( @Id, " + properties_set + " )";
			_readCommand = "select * from " + t.Name + "sTable" ;
			_updateCommand = "update " + t.Name + "sTable"  + " set " + properties_update + " where Id = @Id";
			_deleteCommand = "delete from " + t.Name + "sTable"  + " where Id = @Id";
		}

		public ICrudRepository<T> Create (T model)
		{			
			_connectionFactory.Do((c, t) => c.Execute (_createCommand, model, t));
			return this;
		}

		public ICrudRepository<T> Create(IEnumerable<T> models)
		{
			_connectionFactory.Do((c, t) => c.Execute (_createCommand, models, t));
			return this;
		}

		public IEnumerable<T> Read ()
		{			
			IEnumerable<T> result = Enumerable.Empty<T>();
			_connectionFactory.Do ((c, t) => result = c.Query<T> (_readCommand));
			return result;
		}

		public ICrudRepository<T> Update (T model)
		{
			_connectionFactory.Do ((c, t) => c.Execute (_updateCommand, model, t));
			return this;
		}

		public ICrudRepository<T> Update (IEnumerable<T> models)
		{
			_connectionFactory.Do((c, t) => c.Execute (_updateCommand, models, t));
			return this;
		}


		public ICrudRepository<T> Delete (T model)
		{
			_connectionFactory.Do((c, t) => c.Execute (_deleteCommand, model, t));
			return this;
		}

		public ICrudRepository<T> Delete (IEnumerable<T> models)
		{
			_connectionFactory.Do((c, t) => c.Execute (_deleteCommand, models, t));
			return this;
		}			
	}
}

