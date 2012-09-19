using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	/// <summary>
	/// Add all forbidden words in database here to ensure that no conflicts are made between columns and them.
	/// </summary>
	public class ForbiddenNamesConvention : IPropertyConvention, IPropertyConventionAcceptance
	{
		private readonly IEnumerable<string> _forbiddenNames = new[]
		                                              	{
		                                              		"References",
															//"Name",
															"Username",
															"Table",
															"Schema",
															"Group",
															"Index"
		                                              	};
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => _forbiddenNames.Any(z => z.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)));
		}
		public void Apply(IPropertyInstance instance)
		{
			instance.Column(instance.Name + "_x");
		}
	}
}