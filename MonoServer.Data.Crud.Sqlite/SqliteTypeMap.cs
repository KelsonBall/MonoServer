using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using MonoServer.Data.Crud;

namespace MonoServer.Data.Crud.Sqlite
{		
	public class SqliteTypeMap : ITypeMap
	{
		/// <summary>
		/// Maps CLR types to SQLite Type Affinities.
		/// Note that SQLite has static, weak types.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public string Get(PropertyInfo property)
		{
			switch (property.PropertyType.Name)
			{
			case "Integer":
			case "int":
			case "uint":
			case "Int16":
			case "Int32":
			case "Int64":
			case "UInt16":
			case "UInt32":
			case "UInt64":
			case "long":
			case "short":
			case "ushort":
			case "char":
			case "byte":
			case "sbyte":
				return "INTEGER";
			case "float":
			case "double":
			case "single":
			case "decimal":
				return "REAL";
			case "string":
			case "String":
				if (property.IsDefined(typeof(StringLengthAttribute)))
					return $"VARCHAR({property.GetCustomAttribute<StringLengthAttribute>().MaximumLength})";
				return "TEXT";
			case "byte[]":
			case "Byte[]":
				return "BLOB";
			case "DateTime":
				return "DATETIME";
			default:
				return null;
			}
		}
	}
}

