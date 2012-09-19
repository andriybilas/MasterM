using System.Globalization;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Litium.Infrastructure.DataAccess.UserTypes;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	public class CultureInfoUserTypeConvention : IUserTypeConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Property.PropertyType == typeof(CultureInfo));
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType<CultureInfoType>();
		}
	}
}