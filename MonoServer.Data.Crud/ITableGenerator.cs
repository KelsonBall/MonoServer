using System;

namespace MonoServer.Data.Crud
{
	public interface ITableGenerator
	{
		string GetCreateTableQuery(Type type);
	}
}