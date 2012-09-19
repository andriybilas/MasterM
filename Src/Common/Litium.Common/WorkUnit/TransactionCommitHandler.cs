using System;
using System.Collections.Generic;
using System.Transactions;

namespace Litium.Common.WorkUnit
{
	internal class TransactionCommitHandler : TransactionDefaultHandler
	{
		public TransactionCommitHandler(IList<Action> actions)
			: base(actions)
		{
		}

		public override void Commit(Enlistment enlistment)
		{
			foreach (var action in Actions)
			{
				action.Invoke();
			}
			
			base.Commit(enlistment);
		}
	}
}