using System;
using System.Collections.Generic;
using System.Transactions;

namespace Litium.Common.WorkUnit
{
	internal class TransactionRollbackHandler : TransactionDefaultHandler
	{
		public TransactionRollbackHandler(IList<Action> actions)
			: base(actions)
		{
		}

		public override void Rollback(Enlistment enlistment)
		{
			foreach (var action in Actions)
			{
				action.Invoke();
			}

			base.Rollback(enlistment);
		}
	}
}