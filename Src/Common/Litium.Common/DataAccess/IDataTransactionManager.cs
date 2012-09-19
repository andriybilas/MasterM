using System;

namespace Litium.Common.DataAccess
{
	public interface IDataTransactionManager : IDisposable
	{
		void Initialize();
		void Commit();
		void Rollback();
	}
}
