using Litium.Common.InversionOfControl;
using Litium.Common.Querying;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Querying
{
	[Plugin("Db")]
	public class NHibernateQueryProcessorFactory : IQueryProcessorFactory
	{
		private readonly ISessionFactory _sessionFactory;

		public NHibernateQueryProcessorFactory(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public IQueryProcessor<T> Create<T>()
		{
			return new NHibernateQueryProcessor<T>(_sessionFactory);
		}
	}
}