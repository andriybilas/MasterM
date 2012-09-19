using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Litium.Common.InversionOfControl;
using Litium.Common.Lifecycle;
using Litium.Infrastructure.InversionOfControl;
using log4net.Config;

namespace Litium.Common.Setup
{
	public static class LitiumSetup
	{
		//private static readonly ILog _logger = LogManager.GetLogger(typeof(LitiumSetup));

		public static void Release()
		{
			var releaseTasks = IoC.ResolveAll<IReleaseTask>();
			foreach (var releaseTask in releaseTasks)
			{
				releaseTask.Release();
			}
			IoC.Dispose();
		}

		public static void Setup(IEnumerable<Assembly> assemblies)
		{
			var distinctAssemblies = assemblies.Distinct().ToArray();

			SetupLog();
			SetupInversionOfControl(distinctAssemblies);

			var setupTasks = IoC.ResolveAll<ISetupTask>();
			foreach (var setupTask in setupTasks)
			{
				setupTask.Setup(distinctAssemblies);
			}
		}

		private static void SetupInversionOfControl(Assembly[] assemblies)
		{
			var iocContainer = new WindsorFactory().Create(assemblies);
			iocContainer.For<IIoCContainer>().RegisterInstance(iocContainer);
			iocContainer.InstallAssemblies(assemblies);
			IoC.SetConateiner(iocContainer);
	}

		private static void SetupLog()
		{
			XmlConfigurator.Configure();
		}
	}
}