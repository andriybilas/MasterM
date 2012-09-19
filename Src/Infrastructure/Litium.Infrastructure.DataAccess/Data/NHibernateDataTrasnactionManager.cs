using Litium.Common.DataAccess;
using NHibernate;

namespace Litium.Infrastructure.DataAccess.Data
{
	public class NHibernateDataTrasnactionManager : IDataTransactionManager
	{
		private readonly ISession _session;
		private ITransaction _transaction;

		public NHibernateDataTrasnactionManager(ISession session)
		{
			_session = session;
		}

		public void Initialize()
		{
			_transaction = _session.Transaction.IsActive ? _session.Transaction : _session.BeginTransaction();
		}

		public void Commit()
		{
			if (!_transaction.WasRolledBack)
			{
				_transaction.Commit();
			}
		}

		public void Rollback()
		{
			if (_transaction.IsActive)
			{
				_transaction.Rollback();
			}
		}

		public void Dispose()
		{
			_transaction.Dispose();
		}
	}
}