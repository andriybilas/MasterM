using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	public class CacheableConvention : IClassConvention
	{
		public void Apply(IClassInstance instance)
		{
			instance.Cache.ReadWrite();
			instance.Cache.IncludeAll();
		}
	}
}
