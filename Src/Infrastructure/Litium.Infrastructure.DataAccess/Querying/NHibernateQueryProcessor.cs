using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Litium.Common.Querying;
using NHibernate;
using NHibernate.Linq;

namespace Litium.Infrastructure.DataAccess.Querying
{
	public class NHibernateQueryProcessor<T> : IQueryProcessor<T>
	{
		private readonly ISessionFactory _sessionFactory;

		public NHibernateQueryProcessor(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public QueryResult<T> All(QueryEngineArguments<T> args)
		{
			var query = GetQuery(args).ToFuture();
			return new QueryResult<T>(query);
		}

		public QueryValueResult<int> Count(QueryEngineArguments<T> args)
		{
			var count = GetQuery(args).ToFutureValue(x => x.Count());
			return new QueryValueResult<int>(() => count.Value);
		}

		public QueryValueResult<T> FirstOrDefault(QueryEngineArguments<T> args)
		{
			var value = GetQuery(args).Take(1).ToFuture();
			return new QueryValueResult<T>(value.FirstOrDefault);
		}

		public QueryPagedResult<T> Paging(QueryEngineArguments<T> args, int pageNumber, int pageSize)
		{
			var query = GetQuery(args);
			var count = query.ToFutureValue(x => x.Count());
			var result = query.Skip((pageNumber - 1)*pageSize)
				.Take(pageSize)
				.ToFuture();

			return new QueryPagedResult<T>(result, () => count.Value);
		}

		private IQueryable<T> GetQuery(QueryEngineArguments<T> args)
		{
			var query = _sessionFactory.GetCurrentSession().Query<T>();
			
			//Enable caching for queries.
			query = query.Cacheable();

			query = args.Where.Aggregate(query, (current, @where) => current.Where(where));

			var firstOrder = args.OrderBy.FirstOrDefault();
			if (firstOrder != null)
			{
				args.OrderBy.Remove(firstOrder);
				query = query.OrderBy(firstOrder.PropertyInfo, firstOrder.Descending);
				query = args.OrderBy.Aggregate(query, (current, order) => current.ThenOrderBy(order.PropertyInfo, order.Descending));
			}
			return query;
		}
	}

	public static class NhExpressions
	{
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, PropertyInfo property, bool descending)
		{
			var type = typeof (T);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			MethodCallExpression resultExp = Expression.Call(typeof (Queryable), descending ? "OrderByDescending" : "OrderBy", new[] {type, property.PropertyType}, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<T>(resultExp);
		}

		public static IQueryable<T> ThenOrderBy<T>(this IQueryable<T> source, PropertyInfo property, bool descending)
		{
			var type = typeof (T);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			MethodCallExpression resultExp = Expression.Call(typeof (Queryable), descending ? "ThenByDescending" : "ThenBy", new[] {type, property.PropertyType}, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<T>(resultExp);
		}

		public static IFutureValue<TResult> ToFutureValue<TSource, TResult>(
			this IQueryable<TSource> source,
			Expression<Func<IQueryable<TSource>, TResult>> selector)
			where TResult : struct
		{
			var provider = (INhQueryProvider) source.Provider;
			var method = ((MethodCallExpression) selector.Body).Method;
			var expression = Expression.Call(null, method, new[] {source.Expression});
			return (IFutureValue<TResult>) provider.ExecuteFuture(expression);
		}
	}
}