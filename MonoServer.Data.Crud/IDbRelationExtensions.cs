using System;
using System.Linq;
using System.Collections.Generic;

namespace MonoServer.Data.Crud
{
	public static class IDbRelationExtensions
	{
		public static Tuple<T1,T2> Pair<T1, T2>(this IDbRelation<T1,T2> relation, IDao dao) where T1 : IDbItem where T2 : IDbItem
		{
			var item1 = dao.Get<T1> ().Read ().Single (i => i.Id == relation.Id);
			var item2 = dao.Get<T2> ().Read ().Single (i => i.Id == relation.Id2);
			return new Tuple<T1,T2>(item1, item2);
		}

		public static void Pair<T1, T2>(this IDbRelation<T1,T2> relation, IDao dao, out T1 item1, out T2 item2) where T1 : IDbItem where T2 : IDbItem
		{
			var pair = relation.Pair (dao);
			item1 = pair.Item1;
			item2 = pair.Item2;
		}

		public static IEnumerable<TR> Relations<TR, T1, T2>(this T1 item, IDao dao) where TR : IDbRelation<T1, T2> where T1 : IDbItem where T2 : IDbItem
		{
			return dao.Get<TR> ().Read ().Where (i => i.Id == item.Id);
		}

		public static IEnumerable<TR> Relations<TR, T1, T2>(this T2 item, IDao dao) where TR : IDbRelation<T1, T2> where T1 : IDbItem where T2 : IDbItem
		{
			return dao.Get<TR> ().Read ().Where (i => i.Id2 == item.Id);
		}

		public static IEnumerable<T2> Items<TR, T1, T2>(this T1 item, IDao dao) where TR : IDbRelation<T1, T2> where T1 : IDbItem where T2 : IDbItem
		{
			return item.Relations<TR, T1, T2> (dao).Select (p => p.Pair (dao).Item2);
		}

		public static IEnumerable<T1> Items<TR, T1, T2>(this T2 item, IDao dao) where TR : IDbRelation<T1, T2> where T1 : IDbItem where T2 : IDbItem
		{
			return item.Relations<TR, T1, T2> (dao).Select (p => p.Pair (dao).Item1);
		}
	}
}

