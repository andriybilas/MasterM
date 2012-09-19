using System;
using Litium.Common;
using Litium.Common.Lifecycle;
using Litium.Infrastructure.DataAccess.Cache;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using NHibernate;
using Xunit;

namespace Litium.Test.Common.Xunit.Cache
{
	public class CacheTest : ConversationalTestBase
	{
		[Fact]
		public void EntityCacheTest()
		{
			var sessionFactory = IoC.Resolve<ISessionFactory>();
			//Create and save entity. Reopen conversation to clean 1-st level cache.
			var item = new SimpleCacheEntity
			           	{
			           		Name = "Cache Test #1"
			           	};
			Repository.Data.Save(item);
			ConversationHelper.ReOpen();

			//Check if entity exists in cache directly.
			var cache = GetCachedObjectDirectly(item.Id, item.GetType());
			Assert.NotNull(cache);

			//Check if entity is loaded from cache instead of getting from DB.
			Console.WriteLine("*** No call to DB should be done from this moment ***");
			sessionFactory.Statistics.Clear();
			var retrievedEntity = Repository.Data.Get<SimpleCacheEntity>(item.Id);
			Assert.NotNull(retrievedEntity);
			Assert.Equal(item.Id, retrievedEntity.Id);
			Assert.Equal(item.Name, retrievedEntity.Name);
			Assert.Equal(0, sessionFactory.Statistics.EntityLoadCount);
		}

		[Fact]
		public void QueryCacheTest()
		{
			var sessionFactory = IoC.Resolve<ISessionFactory>();
			//Create and save entities. Reopen conversation to clean 1-st level cache.
			var item1 = new SimpleCacheEntity
			            	{
			            		Name = "Query Cache Test #1"
			            	};
			var item2 = new SimpleCacheEntity
			            	{
			            		Name = "Query Cache Test #2"
			            	};
			Repository.Data.Save(item1);
			Repository.Data.Save(item2);

			ConversationHelper.ReOpen();


			//Query the entities by name
			var query = Repository.Data.Get<SimpleCacheEntity>().Where(x => x.Name.Contains("Query Cache Test"));
			var queryResult = query.All();
			Assert.NotEmpty(queryResult);

			ConversationHelper.ReOpen();

			//Query once again and check if the result is loaded from cache instead of getting from DB.
			Console.WriteLine("*** No call to DB should be done from this moment ***");
			sessionFactory.Statistics.Clear();
			var cachedQueryResult = query.All();
			Assert.NotEmpty(cachedQueryResult);
			Assert.Equal(0, sessionFactory.Statistics.EntityLoadCount);
		}

		[Fact]
		public void ClearCacheForEntityInstanceTest()
		{
			var item1 = new SimpleCacheEntity
			{
				Name = "Cache Test #1"
			};
			var item2 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #2"
			};
			Repository.Data.Save(item1);
			Repository.Data.Save(item2);

			var cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.NotNull(cachedEntity1);
			var cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.NotNull(cachedEntity2);

			Repository.Data.Cache.Clear(item1);

			cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.Null(cachedEntity1);
			cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.NotNull(cachedEntity2);
		}

		[Fact]
		public void ClearCacheForEntityTypeTest()
		{
			var item1 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #1"
			};
			var item2 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #2"
			};
			var item3 = new SimpleEntity
			{
				Name = "Query Cache Test #3"
			};
			Repository.Data.Save(item1);
			Repository.Data.Save(item2);
			Repository.Data.Save(item3);

			var cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.NotNull(cachedEntity1);
			var cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.NotNull(cachedEntity2);
			var cachedEntity3 = GetCachedObjectDirectly(item3.Id, item3.GetType());
			Assert.NotNull(cachedEntity3);

			Repository.Data.Cache.Clear(typeof(SimpleCacheEntity));

			cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.Null(cachedEntity1);
			cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.Null(cachedEntity2);
			cachedEntity3 = GetCachedObjectDirectly(item3.Id, item3.GetType());
			Assert.NotNull(cachedEntity3);
		}

		[Fact]
		public void ClearCacheForQueriesTest()
		{
			var sessionFactory = IoC.Resolve<ISessionFactory>();

			var item1 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #1"
			};
			var item2 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #2"
			};
			Repository.Data.Save(item1);
			Repository.Data.Save(item2);

			var query = Repository.Data.Get<SimpleCacheEntity>().Where(x => x.Name.Contains("Query Cache Test"));
			var queryResult = query.All();
			Assert.NotEmpty(queryResult);
			sessionFactory.Statistics.Clear();
			var cachedQueryResult = query.All();
			Assert.NotEmpty(cachedQueryResult);
			Assert.Equal(0, sessionFactory.Statistics.EntityLoadCount);

			Repository.Data.Cache.ClearQueries();

			var cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.NotNull(cachedEntity1);
			var cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.NotNull(cachedEntity2);
			sessionFactory.Statistics.Clear();
			var retrievedQueryResult = query.All();
			Assert.NotEmpty(retrievedQueryResult);
			Assert.True(sessionFactory.Statistics.EntityLoadCount > 0);
		}

		[Fact]
		public void ClearCacheAllTest()
		{
			var sessionFactory = IoC.Resolve<ISessionFactory>();

			var item1 = new SimpleCacheEntity
			{
				Name = "Query Cache Test #1"
			};
			var item2 = new SimpleEntity
			{
				Name = "Query Cache Test #2"
			};
			Repository.Data.Save(item1);
			Repository.Data.Save(item2);
			
			var cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.NotNull(cachedEntity1);
			var cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.NotNull(cachedEntity2);

			var query = Repository.Data.Get<SimpleCacheEntity>().Where(x => x.Name.Contains("Query Cache Test"));
			var queryResult = query.All();
			Assert.NotEmpty(queryResult);
			sessionFactory.Statistics.Clear();
			var cachedQueryResult = query.All();
			Assert.NotEmpty(cachedQueryResult);
			Assert.Equal(0, sessionFactory.Statistics.EntityLoadCount);

			Repository.Data.Cache.Clear();

			cachedEntity1 = GetCachedObjectDirectly(item1.Id, item1.GetType());
			Assert.Null(cachedEntity1);
			cachedEntity2 = GetCachedObjectDirectly(item2.Id, item2.GetType());
			Assert.Null(cachedEntity2);
			sessionFactory.Statistics.Clear();
			var retrievedQueryResult = query.All();
			Assert.NotEmpty(retrievedQueryResult);
			Assert.True(sessionFactory.Statistics.EntityLoadCount > 0);
		}

		[Fact]
		public void CacheManipulationTest()
		{
			var sessionFactory = IoC.Resolve<ISessionFactory>();

			var item = new SimpleCacheEntity
			{
				Name = "Cache Test #1"
			};
			Repository.Data.Save(item);

			item = Repository.Data.Get<SimpleCacheEntity>(item.Id);
			item.Name = "New name";

			Repository.Data.Save(item);
			sessionFactory.Statistics.Clear();
			item = Repository.Data.Get<SimpleCacheEntity>(item.Id);
			Assert.Equal(0, sessionFactory.Statistics.EntityLoadCount);
			Assert.Equal("New name", item.Name);
		}

		private static object GetCachedObjectDirectly(Guid id, Type type)
		{
			var region = type.ToString();
			var cache = new AppFabricCache(region);
			var key = string.Format("{0}#{1}", type, id);
			var cachedObject = cache.Get(key);
			return cachedObject;
		}
	}
}