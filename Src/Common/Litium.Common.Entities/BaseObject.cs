using System;

namespace Litium.Common.Entities
{
	/// <summary>
	/// 	Base object.
	/// </summary>
	/// <typeparam name="TId"> The type of the id. </typeparam>
	public abstract class BaseObject<TId> : ComparableObject where TId : IComparable
	{
		/// <summary>
		/// 	Gets or sets the id.
		/// </summary>
		/// <value> The id. </value>
		public virtual TId Id { get; protected internal set; }
	}
}
