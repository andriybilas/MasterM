using System;
using System.Collections.Generic;
using System.Transactions;

namespace Litium.Common.WorkUnit
{
	internal abstract class TransactionDefaultHandler : IEnlistmentNotification
	{
		protected readonly IList<Action> Actions;

		protected TransactionDefaultHandler(IList<Action> actions)
		{
			Actions = actions;
		}

		public virtual void Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		public virtual void Commit(Enlistment enlistment)
		{
			enlistment.Done();
		}

		public virtual void Rollback(Enlistment enlistment)
		{
			enlistment.Done();
		}

		public virtual void InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
		}
	}
}