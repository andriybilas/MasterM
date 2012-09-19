using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	public class ComponentNamingConvention : IComponentConvention
	{
		public void Apply(IComponentInstance instance)
		{
			foreach (var property in instance.Properties)
			{
				property.Column(instance.Name + "_" + property.Name);
			}
		}
	}
}
