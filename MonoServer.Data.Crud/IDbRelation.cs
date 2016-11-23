using System;

namespace MonoServer.Data.Crud
{
	public interface IDbRelation<T1,T2> : IDbItem
	{
		string Id2 { get; set; }
	}
}

