using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;
using Mono.Data.Sqlite;
using KelsonBall.Testing;
using MonoServer.Data.Crud.Sqlite;

namespace MonoServer.Data.Crud.Tests
{
	public class Item : IDbItem
	{
		public string Id { get; set; } = Guid.NewGuid ().ToString ();
		public string Content { get; set; }
	}

	[TestClass]
	public class NoRelationModelCrudTests
	{
		private readonly IDao dao = new SqliteDao();

		public NoRelationModelCrudTests ()
		{					
			if (File.Exists ("testdb.sqlite"))
				File.Delete ("testdb.sqlite");
			dao.WithConnection(() => new SqliteConnection ("Data Source=testdb.sqlite;Version=3;FailIfMissing=false"))
			   .WithModelsFrom(Assembly.GetExecutingAssembly());
		}

		[TestMethod]
		public void CreateItemTest()
		{
			Item item = new Item { Content = Guid.NewGuid().ToString() };
			dao.Get<Item> ().Create (item);
			var readItem = dao.Get<Item> ().Read ().Single (i => i.Id == item.Id);
			Assert.True(readItem.Content.Equals(item.Content));
		}

		[TestMethod]
		public void UpdateItemTest()
		{
			Item item = new Item { Content = Guid.NewGuid().ToString() };
			string firstContent = item.Content;
			dao.Get<Item> ().Create (item);
			item.Content = Guid.NewGuid().ToString();
			dao.Get<Item> ().Update (item);
			var readItem = dao.Get<Item> ().Read ().Single (i => i.Id == item.Id);
			Assert.True (readItem.Content.Equals (item.Content));
		}

		[TestMethod]
		public void DeleteItemTest()
		{
			Item item = new Item { Content = Guid.NewGuid().ToString() };
			var items = dao.Get<Item> ();
			items.Create(item);
			Assert.True(items.Read().Count(i => i.Id == item.Id) == 1);
			dao.Get<Item> ().Delete (item);
			Assert.True(items.Read().Count(i => i.Id == item.Id) == 0);
		}
	}
}

