using System.Globalization;
using FluentNHibernate.Testing;
using Litium.Common;
using Litium.Test.Common.Xunit.Base;
using NHibernate;
using Xunit;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleEntityTest : TransactionalTestBase
	{
		[Fact]
		public void PersistSimpleEntity()
		{
			new PersistenceSpecification<SimpleEntity>(IoC.Resolve<ISessionFactory>().GetCurrentSession())
				.CheckProperty(x => x.Name, "Enitty name 12")
				.CheckProperty(x => x.Column1, "Column 1")
				.CheckProperty(x => x.Column2, "Column 2")
				.CheckProperty(x => x.Column3, "Column 3")
				.CheckProperty(x => x.Column4, "Column 4")
				.CheckProperty(x => x.Culture, CultureInfo.CurrentCulture)
				.VerifyTheMappings();
		}
	}
}