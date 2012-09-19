using System;
using System.Linq.Expressions;

namespace Litium.Common.Querying
{
	public interface IQueryFilter<TSource>
	{
		Expression<Func<TSource, bool>> GetFilter();
	}
}