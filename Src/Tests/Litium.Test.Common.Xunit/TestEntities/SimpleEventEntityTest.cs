using FluentNHibernate.Testing;
using Litium.Common;
using Litium.Test.Common.Xunit.Base;
using NHibernate;
using Xunit;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleEventEntityTest : TransactionalTestBase
	{
		[Fact]
		public void PersistSimpleEventEntity()
		{
			new PersistenceSpecification<SimpleEventEntity>(IoC.Resolve<ISessionFactory>().GetCurrentSession())
				.CheckProperty(x => x.Name, "Enitty name 12")
				.VerifyTheMappings();
		}
	}
}