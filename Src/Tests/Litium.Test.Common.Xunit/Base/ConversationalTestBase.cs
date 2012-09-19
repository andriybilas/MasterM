using System;
using System.Linq;
using System.Reflection;
using Litium.Common;
using Litium.Common.Configurations;
using Litium.Common.Lifecycle;
using Litium.Common.Setup;

namespace Litium.Test.Common.Xunit.Base
{
	public abstract class ConversationalTestBase : TestBase
	{
		static ConversationalTestBase()
		{
			LitiumSetup.Setup(LitiumSectionGroup.Instance.Plugin.SolutionAssemblies.Concat(new[]
			                                                                               	{
			                                                                               		typeof (TestBase).Assembly,
			                                                                               		Assembly.GetCallingAssembly()
			                                                                               	}).Distinct());

			AppDomain.CurrentDomain.DomainUnload += (s, e) => LitiumSetup.Release();
		}

		public override void Dispose()
		{
			ConversationHelper.Close();
		}

		protected override void Init()
		{
			ConversationHelper.Open();
			Repository.Data.Cache.Clear();
		}
	}
}
