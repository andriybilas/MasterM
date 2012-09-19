namespace Litium.Common.Querying
{
	public abstract class QueryPaging<TSource> : QueryExecutor<TSource>
	{
		protected QueryPaging(IQueryProcessor<TSource> queryProcessor)
			: base(queryProcessor)
		{
		}

		public QueryPagedResult<TSource> Paging(int pageNumber, int pageSize)
		{
			return QueryProcessor.Paging(GetArgs(), pageNumber, pageSize);
		}
	}
}