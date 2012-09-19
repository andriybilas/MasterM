using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Litium.Common.WorkUnit
{
	internal class UnitOfWorkManager
	{
		private static readonly UnitOfWorkManager _manager = new UnitOfWorkManager();

		private static readonly IDictionary<string, UnitOfWorkScope> _unitOfWorkScopes =
            new ConcurrentDictionary<string, UnitOfWorkScope>();

		private UnitOfWorkManager()
		{
		}

		public static UnitOfWorkManager Instance
		{
			get { return _manager; }
		}

		public UnitOfWorkScope CurrentUnitOfWorkScope
		{
			get
			{
				Transaction currentTransaction;
				try
				{
					currentTransaction = Transaction.Current;
				}
				catch (InvalidOperationException)
				{
					throw new UnitOfWorkException("Unit of work was already completed.");
				}
				if (currentTransaction == null)
					return null;

				UnitOfWorkScope scope;
				if (_unitOfWorkScopes.TryGetValue(currentTransaction.TransactionInformation.LocalIdentifier, out scope))
				{
					if (scope.State == UnitOfWorkState.Committed || scope.State == UnitOfWorkState.RolledBack)
					{
						throw new UnitOfWorkException("Unit of work was already completed.");
					}
					return scope;
				}
				return null;
			}
		}

		public UnitOfWork CurrentUnitOfWork
		{
			get
			{
				var currentScope = CurrentUnitOfWorkScope;
				return currentScope != null
				       	? currentScope.Units.LastOrDefault()
				       	: null;
			}
		}

		public void AddUnitOfWorkScope(string key, UnitOfWorkScope unit)
		{
			if (_unitOfWorkScopes.ContainsKey(key)) return;

			_unitOfWorkScopes.Add(key, unit);
		}

		public void RemoveUnitOfWorkScope(string key)
		{
			if (!_unitOfWorkScopes.ContainsKey(key)) return;

			_unitOfWorkScopes.Remove(key);
		}
	}
}