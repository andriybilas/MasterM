using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Litium.Common.Querying
{
	public class QueryEngineArguments<TSource>
	{
		public QueryEngineArguments()
		{
			Where = new List<Expression<Func<TSource, bool>>>();
			OrderBy = new List<QueryOrder<TSource>.OrderByHolder>();
		}

		public List<QueryOrder<TSource>.OrderByHolder> OrderBy { get; private set; }
		public List<Expression<Func<TSource, bool>>> Where { get; private set; }
	}
}