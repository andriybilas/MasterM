using System;
using System.Collections.Generic;
using System.Transactions;
using Litium.Common.DataAccess;

namespace Litium.Common.WorkUnit
{
	public class UnitOfWork : IDisposable
	{
		private readonly UnitOfWorkScope _unitOfWorkScope;
		private readonly TransactionScope _transactionScope;
		private UnitOfWorkState _state;
		private readonly IList<Action> _preCommitActions;

		public static UnitOfWork Current
		{
			get { return UnitOfWorkManager.Instance.CurrentUnitOfWork; }
		}

		public IList<Action> PreCommitActions
		{
			get { return _preCommitActions; }
		}

		public IList<Action> PostCommitActions
		{
			get { return _unitOfWorkScope.PostCommitActions; }
		}

		public IList<Action> PostRollbackActions
		{
			get { return _unitOfWorkScope.PostRollbackActions; }
		}

		internal IDataAccessRepository Repository
		{
			get { return _unitOfWorkScope.Repository; }
		}

		public UnitOfWork()
			: this(UnitOfWorkScopeType.Default)
		{
		}

		public UnitOfWork(UnitOfWorkScopeType scopeType)
		{
			_preCommitActions = new List<Action>();

			switch (scopeType)
			{
				case UnitOfWorkScopeType.New:
					_transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
					_unitOfWorkScope = new UnitOfWorkScope(scopeType);
					break;
				default:
                    _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
					_unitOfWorkScope = UnitOfWorkManager.Instance.CurrentUnitOfWorkScope ?? new UnitOfWorkScope(scopeType);
					break;
			}
			_unitOfWorkScope.Units.Add(this);
		}

		public void Commit()
		{
			if (_state == UnitOfWorkState.Committed)
			{
				throw new UnitOfWorkException("Can't commit unit of work that was already committed.");
			}

			if (_unitOfWorkScope.State == UnitOfWorkState.RolledBack)
			{
				throw new UnitOfWorkException("Can't commit unit of work that was already rolled back.");
			}

			foreach (var action in _preCommitActions)
			{
				action.Invoke();
			}

			if (IsScopeWrapper)
			{
				_unitOfWorkScope.Commit();
			}

			_transactionScope.Complete();
			_state = UnitOfWorkState.Committed;
		}

		public void Rollback()
		{
			if (_unitOfWorkScope.State == UnitOfWorkState.RolledBack) return;

			if (_unitOfWorkScope.State == UnitOfWorkState.Committed)
			{
				throw new UnitOfWorkException("Can't rollback unit of work that was already committed.");
			}

			_unitOfWorkScope.Rollback();
			_state = UnitOfWorkState.RolledBack;

			if (!IsScopeWrapper)
			{
				throw new UnitOfWorkException("Rolling back inner unit of work will break outer unit of work.");
			}
		}

		public void Dispose()
		{
			UnitOfWorkException unitOfWorkException = null;
			if (_state == UnitOfWorkState.Undefined)
			{
				try
				{
					Rollback();
				}
				catch (UnitOfWorkException ex)
				{
					unitOfWorkException = ex;
				}
			}

			if (IsScopeWrapper)
			{
				_unitOfWorkScope.Dispose();
			}
			else
			{
				_unitOfWorkScope.Units.Remove(this);
			}

			_transactionScope.Dispose();

			if (unitOfWorkException != null)
			{
				throw unitOfWorkException;
			}
		}

		//Is the last unit of work in current scope
		private bool IsScopeWrapper
		{
			get { return _unitOfWorkScope.Units.Count == 1; }
		}
	}
}