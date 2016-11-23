using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Data.Sqlite;
using MonoServer.Data.Crud.Sqlite;
using KelsonBall.Testing;

namespace MonoServer.Data.Crud.Tests
{
	public class Person : IDbItem
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
	}

	public class Table : IDbItem
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public int Capacity { get; set; }
	}

	public class PersonAtTable : DbRelation<Person, Table> 
	{
		public PersonAtTable () : base() { }

		public PersonAtTable (Person p, Table t) : base (p, t) { }
	}

	[TestClass]
	public class RelationTests
	{
		private readonly IDao dao = new SqliteDao();

		public RelationTests ()
		{
			if (File.Exists ("testdb.sqlite"))
				File.Delete ("testdb.sqlite");
			dao.WithConnection(() => new SqliteConnection ("Data Source=testdb.sqlite;Version=3;FailIfMissing=false"))
				.WithModelsFrom(Assembly.GetExecutingAssembly());
		}

		[TestMethod]
		public void AddPersonTableRelation ()
		{
			// Setup
			Person bob = new Person { Name = "Bob" };
			Person sal = new Person { Name = "Sal" };
			Person ann = new Person { Name = "Ann" };

			Table t1 = new Table { Capacity = 4 };
			Table t2 = new Table { Capacity = 2 };

			var people = dao.Get<Person> ();
			var tables = dao.Get<Table> ();
			var peopleAtTables = dao.Get<PersonAtTable> ();

			people.Create (bob, sal, ann);
			tables.Create (t1, t2);

			// Act
			peopleAtTables.Create (new PersonAtTable(bob, t1));
			peopleAtTables.Create (new PersonAtTable (sal, t1));
			peopleAtTables.Create (new PersonAtTable (ann, t2));

			var peopleAtTable1 = t1.Items<PersonAtTable, Person, Table> (dao);
			var peopleAtTable2 = t2.Items<PersonAtTable, Person, Table> (dao);

			// Assert
			Assert.Equals (peopleAtTable1.Count(), 2);
			Assert.Equals (peopleAtTable2.Count (), 1);
			Assert.Equals (peopleAtTable2.Single ().Id, ann.Id);

			// Act
			var salAtTable = peopleAtTables.Read ().Single (t => t.Id == sal.Id);
			peopleAtTables.Delete (salAtTable);

			peopleAtTable1 = t1.Items<PersonAtTable, Person, Table> (dao);

			// Assert 
			Assert.Equals (peopleAtTable1.Count(), 1);
			Assert.Equals (peopleAtTable1.Single ().Id, bob.Id);
		}
	}
}