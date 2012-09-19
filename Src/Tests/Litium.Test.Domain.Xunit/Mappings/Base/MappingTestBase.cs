using Litium.Common;
using Litium.Test.Common.Xunit.Base;
using NHibernate;
using NHibernate.Engine;

namespace Litium.Test.Domain.Xunit.Mappings.Base
{
	public class MappingTestBase : ConversationalTestBase
	{
		protected static ISession Session
		{
			get { return IoC.Resolve<ISessionFactory>().GetCurrentSession(); }
		}
	}
}
