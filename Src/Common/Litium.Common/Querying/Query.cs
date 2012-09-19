using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Litium.Common.Querying
{
	public class Query<TSource> : QueryOrder<TSource>
	{
		private readonly List<Expression<Func<TSource, bool>>> _where = new List<Expression<Func<TSource, bool>>>();

		public Query(IQueryProcessor<TSource> queryProcessor)
			: base(queryProcessor)
		{
		}

		public Query<TSource> Where(Query<TSource> filterQuery)
		{
			var query = (Query<TSource>) GetCopy(null);
			query._where.AddRange(filterQuery._where);
			return query;
		}

		public Query<TSource> Where(IQueryFilter<TSource> filterQuery)
		{
			var predicate = filterQuery.GetFilter();
			return Where(predicate);
		}

		public Query<TSource> Where(Expression<Func<TSource, bool>> predicate)
		{
			var query = (Query<TSource>) GetCopy(null);
			query._where.Add(predicate);
			return query;
		}

		protected override QueryExecutor<TSource> GetCopy(QueryExecutor<TSource> query)
		{
			var newQuery = (Query<TSource>) query ?? new Query<TSource>(QueryProcessor);
			newQuery._where.AddRange(_where);
			return base.GetCopy(newQuery);
		}

		protected override IEnumerable<Expression<Func<TSource, bool>>> GetWhere()
		{
			return base.GetWhere().Concat(_where);
		}

		public static Query<TSource> operator &(Query<TSource> left, Query<TSource> right)
		{
			var query = new Query<TSource>(left.QueryProcessor);
			foreach (var where in left._where.Concat(right._where))
				query.Where(where);
			return query;
		}
	}
}