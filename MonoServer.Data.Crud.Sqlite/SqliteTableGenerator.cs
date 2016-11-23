using System;
using System.Linq;
using System.Reflection;
using MonoServer.Data.Crud;

namespace MonoServer.Data.Crud.Sqlite
{	
	public class SqliteTableGenerator : ITableGenerator
	{
		public string GetCreateTableQuery(Type type)
		{            
			ITypeMap map = new SqliteTypeMap();
			string sep = "," + System.Environment.NewLine;
			var typePairs = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name + " " + map.Get(p));
			return "CREATE TABLE IF NOT EXISTS " + type.Name + "sTable" + " ( " + string.Join(sep, typePairs) + " );";
		}
	}
}


