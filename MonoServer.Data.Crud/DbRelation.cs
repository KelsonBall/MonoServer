using System;

namespace MonoServer.Data.Crud
{
	public class DbRelation<T1,T2> : IDbRelation<T1,T2> where T1 : IDbItem where T2 : IDbItem
	{
		public string Id { get; set; }
		public string Id2 { get; set; }

		public DbRelation()
		{
		}

		public DbRelation(T1 item1, T2 item2)
		{
			Id = item1.Id;
			Id2 = item2.Id;
		}
	}
}