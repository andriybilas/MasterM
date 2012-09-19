using System.Transactions;

namespace Litium.Test.Common.Xunit.Base
{
	public abstract class TransactionalTestBase : ConversationalTestBase
	{
		private TransactionScope _transaction;

		protected override void Init()
		{
			_transaction = new TransactionScope();

			base.Init();
		}

		public override void Dispose()
		{
			base.Dispose();

			if (_transaction != null)
				_transaction.Dispose();
		}
	}
}
