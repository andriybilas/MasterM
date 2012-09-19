using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Litium.Common;
using Litium.Common.Querying;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.Querying
{
	public class QueriesTest : ConversationalTestBase
	{
		[Fact]
		public void AllQueryPossibilities()
		{
			var query = Repository.Data.Get<SimpleEntity>();
			Assert.IsType<Query<SimpleEntity>>(query);

			query = query.Where(x => x.Id == Guid.NewGuid());

			var queryWithIncludeQuery = Repository.Data.Get<SimpleEntity>().Where(query);
			var queryWithFilter = Repository.Data.Get<SimpleEntity>().Where(new PublishedQueryFilter());
			var queryAndQueryAndQuery = queryWithIncludeQuery & queryWithFilter;

			var queryWithOrder = queryAndQueryAndQuery.OrderBy(x => x.Column1).OrderByDescending(x => x.Column2);

			var pagedQuery = queryWithOrder.Paging(2, 10);
			Assert.IsType<QueryPagedResult<SimpleEntity>>(pagedQuery);

			var allItems = query.All();
			Assert.IsAssignableFrom<QueryResult<SimpleEntity>>(allItems);

			var firstItem = query.FirstOrDefault();
			Assert.IsType<QueryValueResult<SimpleEntity>>(firstItem);

			var pageCount = query.Count();
			Assert.IsType<QueryValueResult<int>>(pageCount);
		}

		[Fact]
		public void CanGetByColumn()
		{
			var item = new SimpleEntity { Culture = Thread.CurrentThread.CurrentUICulture };
			Repository.Data.Save(item);
			Assert.NotEqual(Guid.Empty, item.Id);

			SimpleEntity item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Culture == CultureInfo.GetCultureInfo("es-ES")).FirstOrDefault().Value;
			Assert.Null(item2);

			item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Culture == Thread.CurrentThread.CurrentUICulture).FirstOrDefault().Value;
			Assert.NotNull(item2);
		}

		[Fact]
		public void CanGetById()
		{
			var item = new SimpleEntity { Culture = Thread.CurrentThread.CurrentUICulture };
			Repository.Data.Save(item);
			Assert.NotEqual(Guid.Empty, item.Id);

			SimpleEntity item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Id == Guid.NewGuid()).FirstOrDefault().Value;
			Assert.Null(item2);

			item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Id == item.Id).FirstOrDefault().Value;
			Assert.NotNull(item2);
		}
		
		[Fact]
		public void CanGetEqualById()
		{
			var item = new SimpleEntity { Culture = Thread.CurrentThread.CurrentUICulture };
			Repository.Data.Save(item);
			Assert.NotEqual(Guid.Empty, item.Id);

			SimpleEntity item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Id.Equals(Guid.NewGuid())).FirstOrDefault().Value;
			Assert.Null(item2);

			item2 = Repository.Data.Get<SimpleEntity>().Where(x => x.Id.Equals(item.Id)).FirstOrDefault().Value;
			Assert.NotNull(item2);
		}

		[Fact]
		public void QueryOrder()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.OrderBy(x => x.Name)
				.All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryOrderDescending()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.OrderByDescending(x => x.Name)
				.All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryOrderMultiOrder()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.OrderBy(x => x.Name)
				.OrderByDescending(x => x.Column1)
				.OrderBy(x => x.Column3)
				.OrderByDescending(x => x.Column4)
				.All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryOrderPaging()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.OrderBy(x => x.Name)
				.Paging(2, 5);

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryPaging()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Paging(2, 5);

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWhere()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Where(x => x.Column2 == "My value")
				.All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWhereCombine()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Where(Repository.Data.Get<SimpleEntity>().Where(x => x.Column2 == "Value"))
				.All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWhereFilter()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Where(new PublishedQueryFilter())
				.All();
			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWhereOperator()
		{
			var result = (Repository.Data.Get<SimpleEntity>() & Repository.Data.Get<SimpleEntity>().Where(x => x.Column2 == "value")).All();

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWhereOrderPaging()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Where(x => x.Column1 == "EntityName")
				.OrderBy(x => x.Name)
				.Paging(2, 5);

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		[Fact]
		public void QueryWherePaging()
		{
			var result = Repository.Data.Get<SimpleEntity>()
				.Where(x => x.Column1 == "Entity")
				.Paging(2, 5);

			Assert.NotNull(result);
			Assert.NotEqual(-1, result.Count());
		}

		public class PublishedQueryFilter : IQueryFilter<SimpleEntity>
		{
			public Expression<Func<SimpleEntity, bool>> GetFilter()
			{
				return x => x.Column4 == "Col4";
			}
		}
	}
}