using Litium.Common.DataAccess;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Data
{
	public class NHibernateRepositoryManager : IDataRepositoryManager
	{
		private readonly ISessionFactory _sessionFactory;

		public NHibernateRepositoryManager(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public IDataAccessRepository GetCurrentRepository()
		{
			return new NHibernateRepository(_sessionFactory.GetCurrentSession());
		}

		public IDataAccessRepository GetNewRepository()
		{
			return new NHibernateRepository(_sessionFactory.OpenSession());
		}
	}
}