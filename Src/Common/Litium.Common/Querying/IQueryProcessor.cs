namespace Litium.Common.Querying
{
	public interface IQueryProcessor<TSource>
	{
		QueryResult<TSource> All(QueryEngineArguments<TSource> args);
		QueryValueResult<int> Count(QueryEngineArguments<TSource> args);
		QueryValueResult<TSource> FirstOrDefault(QueryEngineArguments<TSource> args);
		QueryPagedResult<TSource> Paging(QueryEngineArguments<TSource> args, int pageNumber, int pageSize);

		//void OrderBy<TKey>(Expression<Func<TSource, TKey>> predicate);
		//void OrderByDescending<TKey>(Expression<Func<TSource, TKey>> predicate);
		//void Where(Expression<Func<TSource, bool>> predicate);
	}
}