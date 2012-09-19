using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	public class NotNullObjectConvention : IPropertyConvention, IPropertyConventionAcceptance
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Type == typeof(bool)
				|| x.Type == typeof(DateTime)
				|| x.Type == typeof(int)
				|| x.Type == typeof(long)
				|| x.Type == typeof(short)
				|| x.Type == typeof(decimal)
				|| x.Type == typeof(float)
				);
		}
		public void Apply(IPropertyInstance instance)
		{
			instance.Not.Nullable();
		}
	}
}