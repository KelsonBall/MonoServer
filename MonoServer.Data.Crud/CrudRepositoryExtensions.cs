using System;

namespace MonoServer.Data.Crud
{
	public static class CrudRepositoryExtensions
	{
		public static ICrudRepository<T> Create<T>(this ICrudRepository<T> repo, params T[] items) where T : IDbItem
		{
			repo.Create (items);
			return repo;
		}

		public static ICrudRepository<T> Update<T>(this ICrudRepository<T> repo, params T[] items) where T : IDbItem
		{
			repo.Update (items);
			return repo;
		}

		public static ICrudRepository<T> Delete<T>(this ICrudRepository<T> repo, params T[] items) where T : IDbItem
		{
			repo.Delete (items);
			return repo;
		}
	}
}

