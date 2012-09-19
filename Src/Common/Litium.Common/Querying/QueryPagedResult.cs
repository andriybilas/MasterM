using System;
using System.Collections.Generic;

namespace Litium.Common.Querying
{
	public class QueryPagedResult<TSource> : QueryResult<TSource>
	{
		private readonly Lazy<int> _totalHits;

		public QueryPagedResult(IEnumerable<TSource> items, Func<int> totalHits)
			: base(items)
		{
			_totalHits = new Lazy<int>(totalHits);
		}

		public int TotalHits
		{
			get { return _totalHits.Value; }
		}
	}
}