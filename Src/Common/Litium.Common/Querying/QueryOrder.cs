using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Litium.Common.Entities.Extensions;

namespace Litium.Common.Querying
{
	public abstract class QueryOrder<TSource> : QueryPaging<TSource>
	{
		private readonly List<OrderByHolder> _orderBy = new List<OrderByHolder>();

		protected QueryOrder(IQueryProcessor<TSource> queryProcessor)
			: base(queryProcessor)
		{
		}

		public QueryOrder<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> predicate)
		{
			var query = (QueryOrder<TSource>) GetCopy(null);
			query._orderBy.Add(new OrderByHolder
			                   	{
			                   		PropertyInfo = predicate.GetPropertyInfo()
			                   	});
			return query;
		}

		public QueryOrder<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> predicate)
		{
			var query = (QueryOrder<TSource>) GetCopy(null);
			query._orderBy.Add(new OrderByHolder
			                   	{
			                   		Descending = true,
			                   		PropertyInfo = predicate.GetPropertyInfo()
			                   	});
			return query;
		}

		protected override QueryExecutor<TSource> GetCopy(QueryExecutor<TSource> query)
		{
			var newQuery = (QueryOrder<TSource>) query;
			newQuery._orderBy.AddRange(_orderBy);
			return base.GetCopy(newQuery);
		}

		protected override IEnumerable<OrderByHolder> GetOrderBy()
		{
			return base.GetOrderBy().Concat(_orderBy);
		}

		public class OrderByHolder
		{
			public bool Descending { get; set; }
			public PropertyInfo PropertyInfo { get; set; }
		}
	}
}