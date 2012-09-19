using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	/// <summary>
	/// Enums should be stored as int in databaes
	/// </summary>
	public class EnumConversion : IUserTypeConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Type.IsEnum);
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType<int>();
			instance.Not.Nullable();
		}
	}
}
