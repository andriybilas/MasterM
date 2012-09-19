using System;
using System.Collections.Generic;
using System.Transactions;
using Litium.Common.DataAccess;

namespace Litium.Common.WorkUnit
{
	internal class UnitOfWorkScope : IDisposable
	{
		private readonly string _transactionId;
		private readonly IList<UnitOfWork> _units;
		private readonly IList<Action> _postCommitActions;
		private readonly IList<Action> _postRollbackActions;
		private readonly IDataAccessRepository _repository;
		private readonly UnitOfWorkScopeType _scopeType;

		internal IList<UnitOfWork> Units
		{
			get { return _units; }
		}

		public IList<Action> PostCommitActions
		{
			get { return _postCommitActions; }
		}

		public IList<Action> PostRollbackActions
		{
			get { return _postRollbackActions; }
		}

		internal IDataAccessRepository Repository
		{
			get { return _repository; }
		}

		internal UnitOfWorkState State { get; private set; }

		internal UnitOfWorkScope(UnitOfWorkScopeType scopeType)
		{
			_scopeType = scopeType;
			_units = new List<UnitOfWork>();
			_postCommitActions = new List<Action>();
			_postRollbackActions = new List<Action>();

			var repositoryHelper = IoC.Resolve<IDataRepositoryManager>();
			switch (scopeType)
			{
				case UnitOfWorkScopeType.New:
					_repository = repositoryHelper.GetNewRepository();
					break;
				default:
					_repository = repositoryHelper.GetCurrentRepository();
					break;
			}
			_repository.Transaction.Initialize();
			_transactionId = Transaction.Current.TransactionInformation.LocalIdentifier;
			UnitOfWorkManager.Instance.AddUnitOfWorkScope(_transactionId, this);
		}

		internal void Commit()
		{
			if (State == UnitOfWorkState.RolledBack)
			{
				throw new UnitOfWorkException("Can't commit unit of work that was already rolled back.");
			}

			_repository.Transaction.Commit();

			if (Transaction.Current != null && Transaction.Current.TransactionInformation.Status != TransactionStatus.Aborted)
			{
				Transaction.Current.EnlistVolatile(new TransactionCommitHandler(_postCommitActions), EnlistmentOptions.None);
			}

			State = UnitOfWorkState.Committed;
		}

		public void Rollback()
		{
			if (State == UnitOfWorkState.RolledBack) return;
			
			if (State == UnitOfWorkState.Committed)
			{
				throw new UnitOfWorkException("Can't rollback unit of work that was already committed.");
			}

			_repository.Transaction.Rollback();

			if (Transaction.Current != null)
			{
				Transaction.Current.EnlistVolatile(new TransactionRollbackHandler(_postRollbackActions), EnlistmentOptions.None);
				Transaction.Current.Rollback();
			}

			State = UnitOfWorkState.RolledBack;
		}

		public void Dispose()
		{
			UnitOfWorkManager.Instance.RemoveUnitOfWorkScope(_transactionId);

			_repository.Transaction.Dispose();

			if (_scopeType == UnitOfWorkScopeType.New)
			{
				_repository.Dispose();
			}
		}
	}
}