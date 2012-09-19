using System.Linq;
using NHibernate.Linq;

namespace Litium.Infrastructure.DataAccess.Extensions
{
	public static class QueryExtension
	{
		public static IQueryable<T> Cache<T>(this IQueryable<T> query, bool useCache)
		{
			return useCache ? query.Cacheable() : query;
		}
	}
}
