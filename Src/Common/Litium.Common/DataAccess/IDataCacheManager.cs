using System;
using Litium.Common.Entities;

namespace Litium.Common.DataAccess
{
	public interface IDataCacheManager
	{
		/// <summary>
		/// Removes entity from cache.
		/// </summary>
		void Clear(Entity entity);

		/// <summary>
		/// Removes all entities of specific type from cache.
		/// </summary>
		void Clear(Type entityType);

		/// <summary>
		/// Removes all queries from cache.
		/// </summary>
		void ClearQueries();

		/// <summary>
		/// Removes everything from cache.
		/// </summary>
		void Clear();
	}
}
