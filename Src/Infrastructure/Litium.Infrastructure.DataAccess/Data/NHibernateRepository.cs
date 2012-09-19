using Litium.Common.DataAccess;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Data
{
	/// <summary>
	/// Repository using NHibernate to provide CRUD functionality for all entties
	/// </summary>
	public class NHibernateRepository : IDataAccessRepository
	{
		private readonly ISession _session;
		private readonly NHibernateDataCacheManager _cacheManager;
		private readonly NHibernateDataTrasnactionManager _trasnactionManager;
		private static volatile object _lock = new object();

		/// <summary>
		/// Cache manager
		/// </summary>
		public IDataCacheManager Cache
		{
			get { return _cacheManager; }
		}

		/// <summary>
		/// Transaction manager
		/// </summary>
		public IDataTransactionManager Transaction
		{
			get { return _trasnactionManager; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="session">NHibernate session</param>
		public NHibernateRepository(ISession session)
		{
			_session = session;
			_cacheManager = new NHibernateDataCacheManager(session);
			_trasnactionManager = new NHibernateDataTrasnactionManager(session);
		}

		/// <summary>
		/// Gets entity by its identifier. 
		/// Should be called when not sure if entity exists (return null if entity doesn't exist).
		/// Makes direct request to DB when it's called.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="id">Identifier of entity</param>
		/// <returns>Entity of T type</returns>
		public T Get<T>(object id)
		{
			lock (_lock)
			{
				return _session.Get<T>(id);
			}
		}

		/// <summary>
		/// Loads entity by its identifier. 
		/// Should be called when sure that entity exists, otherwise exception will be thrown (never return null).
		/// Doesn't hit DB directly when it's called, can return a proxy.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="id">Identifier of entity</param>
		/// <returns>Entity of T type</returns>
		public T Load<T>(object id)
		{
			return _session.Load<T>(id);
		}

		/// <summary>
		/// Saves (inserts or updates) entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Save(object entity)
		{
			try
			{
				lock(_lock)
				{
					_session.SaveOrUpdate(entity);	
				}
			}
			catch(NonUniqueObjectException)
			{
				_session.Merge(entity);
			}
		}

		/// <summary>
		/// Inserts entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(object entity)
		{
			try
			{
				lock(_lock)
				{
					_session.Save(entity);	
				}
			}
			catch (NonUniqueObjectException)
			{
				_session.Merge(entity);
			}
		}

		/// <summary>
		/// Updates entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Update(object entity)
		{
			try
			{
				lock(_lock)
				{
					_session.Update(entity);	
				}
			}
			catch (NonUniqueObjectException)
			{
				_session.Merge(entity);
			}
		}

		/// <summary>
		/// Deletes entity from DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Delete(object entity)
		{
			lock(_lock)
			{
				_session.Delete(entity);	
			}
		}

		/// <summary>
		/// Disposes the session
		/// </summary>
		public void Dispose()
		{
			_session.Dispose();
		}
	}
}