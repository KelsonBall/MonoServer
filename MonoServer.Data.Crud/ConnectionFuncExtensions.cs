using System;
using System.Data;
using System.Data.SqlClient;

namespace MonoServer.Data.Crud
{
	public static class ConnectionFuncExtensions
	{
		public static void Do(this Func<IDbConnection> connectionFactory, Action<IDbConnection, IDbTransaction> operation)
		{
			using (IDbConnection connection = connectionFactory ()) 
			{
				connection.Open ();
				using (IDbTransaction transaction = connection.BeginTransaction ()) 
				{
					try 
					{
						operation(connection, transaction);
						transaction.Commit ();
					} 
					catch (SqlException) 
					{
						transaction.Rollback ();
						throw;
					}
				}
				connection.Close ();
			}
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
		}
	}
}

