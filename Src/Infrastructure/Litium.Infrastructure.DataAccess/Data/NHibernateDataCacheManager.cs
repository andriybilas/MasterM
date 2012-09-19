using System;
using Litium.Common.DataAccess;
using Litium.Common.Entities;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Data
{
	public class NHibernateDataCacheManager : IDataCacheManager
	{
		private readonly ISession _session;

		public NHibernateDataCacheManager(ISession session)
		{
			_session = session;
		}

		public void Clear(Entity entity)
		{
			_session.Evict(entity);
			_session.SessionFactory.Evict(entity.GetType(), entity.Id);
		}

		public void Clear(Type type)
		{
			_session.Clear();
			_session.SessionFactory.Evict(type);
		}

		public void ClearQueries()
		{
			_session.Clear();
			_session.SessionFactory.EvictQueries();
		}

		public void Clear()
		{
			//Clear 1-st level caching
			_session.Clear();

			//Clear all cached queries
			_session.SessionFactory.EvictQueries();

			//Clear all cached entities
			var allClassMetadata = _session.SessionFactory.GetAllClassMetadata();
			foreach (var classMetadata in allClassMetadata)
			{
				_session.SessionFactory.EvictEntity(classMetadata.Key);
			}

			//Clear all cached collections
			var allCollectionMetadata = _session.SessionFactory.GetAllCollectionMetadata();
			foreach (var collectionMetadata in allCollectionMetadata)
			{
				_session.SessionFactory.EvictCollection(collectionMetadata.Key);
			}
		}
	}
}