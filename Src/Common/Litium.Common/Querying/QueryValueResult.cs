using System;

namespace Litium.Common.Querying
{
	/// <summary>
	/// 	Single value result from query.
	/// </summary>
	/// <typeparam name = "TSource">The type of the source.</typeparam>
	public class QueryValueResult<TSource>
	{
		private readonly Lazy<TSource> _item;

		/// <summary>
		/// 	Initializes a new instance of the <see cref = "QueryValueResult&lt;TSource&gt;" /> class.
		/// </summary>
		/// <param name = "item">The item.</param>
		public QueryValueResult(Func<TSource> item)
		{
			_item = new Lazy<TSource>(item);
		}

		/// <summary>
		/// 	Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public TSource Value
		{
			get { return _item.Value; }
		}
	}
}