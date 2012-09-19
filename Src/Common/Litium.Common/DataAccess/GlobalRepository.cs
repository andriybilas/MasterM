using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Litium.Common.Querying;
using Litium.Common.WorkUnit;

namespace Litium.Common.DataAccess
{
	public class GlobalRepository
	{
		private readonly IDataRepositoryManager _repositoryManager;

		private IDataAccessRepository DataAccessRepository
		{
			get
			{
				var unitOfWork = UnitOfWork.Current;
				return unitOfWork != null
						? unitOfWork.Repository
				       	: _repositoryManager.GetCurrentRepository();
			}
		}

		/// <summary>
		/// Cache manager
		/// </summary>
		public IDataCacheManager Cache
		{
			get { return DataAccessRepository.Cache; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="repositoryManager">Repository manager</param>
		public GlobalRepository(IDataRepositoryManager repositoryManager)
		{
			_repositoryManager = repositoryManager;
		}

		/// <summary>
		/// Gets entity by its identifier.
		/// Makes direct request to DB when it's called. Return null if entity doesn't exist.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="id">Identifier of entity</param>
		/// <returns>Entity of T type</returns>
		public T Get<T>(object id)
		{
			if (id == null)
				throw new ArgumentNullException("id");

			return ExecuteInUnitOfWork(() => DataAccessRepository.Get<T>(id));
		}

		public Query<T> Find<T>() where T : class
		{
			var queryEngineFactory = IoC.ResolvePlugin<IQueryProcessorFactory>("Search");
			if (queryEngineFactory == null)
				throw new Exception("Cant find any query engine for search");

			var queryEngine = queryEngineFactory.Create<T>();

			return new Query<T>(queryEngine);
		}

		public Query<T> Get<T>() where T : class
		{
			var queryEngineFactory = IoC.ResolvePlugin<IQueryProcessorFactory>("Db");
			if (queryEngineFactory == null)
				throw new Exception("Cant find any query engine for database.");

			var queryEngine = queryEngineFactory.Create<T>();

			return new Query<T>(queryEngine);
		}

		/// <summary>
		/// Saves (inserts or updates) entity in DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Save(object entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			ExecuteInUnitOfWork(() => DataAccessRepository.Save(entity));
		}

		/// <summary>
		/// Saves (inserts or updates) entities in DB.
		/// </summary>
		/// <param name="entities">Entities</param>
		public void Save(params object[] entities)
		{
			if (entities == null)
				throw new ArgumentNullException("entities");

			if (entities.Length == 0)
				return;

			ExecuteInUnitOfWork(() =>
			                    	{
			                    		foreach (var entity in entities)
			                    		{
			                    			Save(entity);
			                    		}
			                    	});
		}

		/// <summary>
		/// Saves (inserts or updates) entities in DB.
		/// </summary>
		/// <param name="entities">Entities</param>
		public void Save(IEnumerable<object> entities)
		{
			if (entities == null)
				throw new ArgumentNullException("entities");

			Save(entities.ToArray());
		}

		/// <summary>
		/// Deletes entity from DB.
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Delete(object entity)
		{
			if (entity == null) 
				throw new ArgumentNullException("entity");

			ExecuteInUnitOfWork(() => DataAccessRepository.Delete(entity));
		}

		/// <summary>
		/// Deletes entities from DB.
		/// </summary>
		/// <param name="entities">Entities</param>
		public void Delete(params object[] entities)
		{
			if (entities == null)
				throw new ArgumentNullException("entities");

			if (entities.Length == 0) 
				return;

			ExecuteInUnitOfWork(() =>
			                    	{
			                    		foreach (var entity in entities)
			                    		{
			                    			Delete(entity);
			                    		}
			                    	});
		}

		/// <summary>
		/// Deletes entities from DB.
		/// </summary>
		/// <param name="entities">Entities</param>
		public void Delete(IEnumerable<object> entities)
		{
			if (entities == null)
				throw new ArgumentNullException("entities");

			Delete(entities.ToArray());
		}

		/// <summary>
		/// Deletes entities filtered by expression from DB.
		/// </summary>
		/// <param name="filter">Filter for entities to delete</param>
		public void Delete<T>(Expression<Func<T, bool>> filter) where T : class
		{
			if (filter == null)
				throw new ArgumentNullException("filter");

			var entities = Get<T>().Where(filter).All();
			Delete(entities);
		}

		private static T ExecuteInUnitOfWork<T>(Func<T> action)
		{
			T result;
			if (UnitOfWork.Current != null)
			{
				result = action.Invoke();
			}
			else
			{
				using (var unitOfWork = new UnitOfWork())
				{
					result = action.Invoke();
					unitOfWork.Commit();
				}
			}
			return result;
		}

		private static void ExecuteInUnitOfWork(Action action)
		{
			ExecuteInUnitOfWork<object>(() =>
			                            	{
			                            		action.Invoke();
			                            		return null;
			                            	});
		}

		//These are extra methods that are not currently used but could be usefull in future
		//TODO: Remove if isn't used

		/////// <summary>
		/////// Loads entity by its identifier. 
		/////// Should be called when sure that entity exists, otherwise exception will be thrown (never return null).
		/////// Doesn't hit DB directly when it's called, can return a proxy.
		/////// </summary>
		/////// <typeparam name="T">Type of entity</typeparam>
		/////// <param name="id">Identifier of entity</param>
		/////// <returns>Entity of T type</returns>
		////public T Load<T>(object id)
		////{
		////    return DataAccessRepository.Load<T>(id);
		////}

		///// <summary>
		///// Inserts entity in DB.
		///// </summary>
		///// <param name="entity">Entity</param>
		//public void Insert(object entity)
		//{
		//    ExecuteInUnitOfWork(() => DataAccessRepository.Insert(entity));
		//}

		///// <summary>
		///// Inserts entities in DB.
		///// </summary>
		///// <param name="entities">Entities</param>
		//public void Insert(params object[] entities)
		//{
		//    ExecuteInUnitOfWork(() =>
		//                            {
		//                                foreach (var entity in entities)
		//                                {
		//                                    Insert(entity);
		//                                }
		//                            });
		//}

		///// <summary>
		///// Inserts entities in DB.
		///// </summary>
		///// <param name="entities">Entities</param>
		//public void Insert(IEnumerable<object> entities)
		//{
		//    ExecuteInUnitOfWork(() =>
		//                            {
		//                                foreach (var entity in entities)
		//                                {
		//                                    Insert(entity);
		//                                }
		//                            });
		//}

		///// <summary>
		///// Updates entity in DB.
		///// </summary>
		///// <param name="entity">Entity</param>
		//public void Update(object entity)
		//{
		//    ExecuteInUnitOfWork(() => DataAccessRepository.Update(entity));
		//}

		///// <summary>
		///// Updates entities in DB.
		///// </summary>
		///// <param name="entities">Entities</param>
		//public void Update(params object[] entities)
		//{
		//    ExecuteInUnitOfWork(() =>
		//                            {
		//                                foreach (var entity in entities)
		//                                {
		//                                    Update(entity);
		//                                }
		//                            });
		//}

		///// <summary>
		///// Updates entities in DB.
		///// </summary>
		///// <param name="entities">Entities</param>
		//public void Update(IEnumerable<object> entities)
		//{
		//    ExecuteInUnitOfWork(() =>
		//                            {
		//                                foreach (var entity in entities)
		//                                {
		//                                    Update(entity);
		//                                }
		//                            });
		//}
	}
}