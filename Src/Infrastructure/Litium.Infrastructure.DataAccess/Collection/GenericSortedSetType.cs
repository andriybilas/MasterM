using System;
using System.Collections.Generic;

namespace Litium.Infrastructure.DataAccess.Collection
{
	[Serializable]
	public class GenericSortedSetType<T> : GenericSetType<T>
	{
		private readonly IComparer<T> comparer;

		public GenericSortedSetType(string role, string propertyRef, IComparer<T> comparer)
			: base(role, propertyRef)
		{
			this.comparer = comparer;
		}

		public IComparer<T> Comparer
		{
			get { return comparer; }
		}

		public override object Instantiate(int anticipatedSize)
		{
			return new SortedSet<T>(comparer);
		}
	}
}