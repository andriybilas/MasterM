using System.Collections;
using System.Collections.Generic;

namespace Litium.Common.Querying
{
	/// <summary>
	/// 	Query results.
	/// </summary>
	/// <typeparam name = "TSource">The type of the source.</typeparam>
	public class QueryResult<TSource> : IEnumerable<TSource>
	{
		private readonly IEnumerable<TSource> _items;

		/// <summary>
		/// 	Initializes a new instance of the <see cref = "QueryResult&lt;TSource&gt;" /> class.
		/// </summary>
		/// <param name = "items">The items.</param>
		public QueryResult(IEnumerable<TSource> items)
		{
			_items = items;
		}

		/// <summary>
		/// 	Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// 	An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<TSource> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		/// <summary>
		/// 	Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// 	An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TSource>) this).GetEnumerator();
		}
	}
}