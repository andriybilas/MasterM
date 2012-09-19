namespace Litium.Common.Querying
{
	public interface IQueryProcessorFactory
	{
		IQueryProcessor<T> Create<T>();
	}
}