using NHibernate.Linq.Functions;

namespace Litium.Infrastructure.DataAccess.LinqFunctions
{
	public class LitiumLinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
	{
		public LitiumLinqToHqlGeneratorsRegistry()
		{
			this.Merge(new LitiumEqualsGenerator());
			this.Merge(new LitiumBoolEqualsGenerator());
		}
	}
}