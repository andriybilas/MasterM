using System.Collections.Generic;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	public static class EnumerableExtenssions
	{
		public static ISet<T> ToSet<T>(this IEnumerable<T> source)
		{
			return new HashSet<T>(source);
		}
	}
}