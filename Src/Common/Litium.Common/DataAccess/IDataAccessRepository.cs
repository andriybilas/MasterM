using System;

namespace Litium.Common.DataAccess
{
	/// <summary>
	/// Interface to provide CRUD functionality for all entties
	/// </summary>
	public interface IDataAccessRepository : IDisposable
	{
		/// <summary>
		/// Gets entity by its identifier. 
		/// Should be called when not sure if entity exists (return null if entity doesn't exist).
		/// Makes direct request to DB when it's called.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="id">Identifier of entity</param>
		/// <returns>Entity of T type</returns>
		T Get<T>(object id);

		/// <summary>
		/// Loads entity by its identifier. 
		/// Should be called when sure that entity exists, otherwise exception will be thrown (never return null).
		/// Doesn't hit DB directly when it's called, can return a proxy.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="id">Identifier of entity</param>
		/// <returns>Entity of T type</returns>
		T Load<T>(object id);

		/// <summary>
		/// Saves (inserts or updates) entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Save(object entity);

		/// <summary>
		/// Inserts entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Insert(object entity);

		/// <summary>
		/// Updates entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Update(object entity);

		/// <summary>
		/// Deletes entity from DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Delete(object entity);

		/// <summary>
		/// Cache manager
		/// </summary>
		IDataCacheManager Cache { get; }

		/// <summary>
		/// Transaction manager
		/// </summary>
		IDataTransactionManager Transaction { get; }
	}
}