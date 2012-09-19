namespace Litium.Common.DataAccess
{
	public interface IDataRepositoryManager
	{
		IDataAccessRepository GetCurrentRepository();
		IDataAccessRepository GetNewRepository();
	}
}
