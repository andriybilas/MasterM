using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Litium.Common.Querying
{
	public abstract class QueryExecutor<TSource>
	{
		protected readonly IQueryProcessor<TSource> QueryProcessor;

		protected QueryExecutor(IQueryProcessor<TSource> queryProcessor)
		{
			QueryProcessor = queryProcessor;
		}

		public QueryResult<TSource> All()
		{
			return QueryProcessor.All(GetArgs());
		}

		public QueryValueResult<int> Count()
		{
			return QueryProcessor.Count(GetArgs());
		}

		public QueryValueResult<TSource> FirstOrDefault()
		{
			return QueryProcessor.FirstOrDefault(GetArgs());
		}

		protected QueryEngineArguments<TSource> GetArgs()
		{
			var args = new QueryEngineArguments<TSource>();
			args.Where.AddRange(GetWhere());
			args.OrderBy.AddRange(GetOrderBy());
			return args;
		}

		protected virtual QueryExecutor<TSource> GetCopy(QueryExecutor<TSource> query)
		{
			return query;
		}

		protected virtual IEnumerable<QueryOrder<TSource>.OrderByHolder> GetOrderBy()
		{
			return new List<QueryOrder<TSource>.OrderByHolder>();
		}

		protected virtual IEnumerable<Expression<Func<TSource, bool>>> GetWhere()
		{
			return new List<Expression<Func<TSource, bool>>>();
		}
	}
}