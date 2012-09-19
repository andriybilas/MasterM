using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Litium.Common.InversionOfControl;
using log4net;

namespace Litium.Infrastructure.InversionOfControl
{
	public static class IoCContainerExtensions
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof (IoCContainerExtensions));

		/// <summary>
		/// 	Installs the assemblies.
		/// </summary>
		/// <param name = "container">The container.</param>
		/// <param name = "assemblies">The assemblies.</param>
		public static void InstallAssemblies(this IIoCContainer container, Assembly[] assemblies)
		{
			//Contract.Requires(container != null);
			//Contract.Requires(assemblies != null);

			using (var registrationContainer = new WindsorContainer())
			{
				foreach (var assembly in assemblies)
				{
					try
					{
						//container.Install(FromAssembly.Instance(assembly));
						registrationContainer.Register(AllTypes.FromAssembly(assembly)
						                               	.IncludeNonPublicTypes()
						                               	.BasedOnAsService<IComponentInstaller>()
						                               	.Configure(c => c.LifeStyle.Transient));
					}
					catch (Exception e)
					{
						var msg = String.Format("Could not load assembly '{0}' because related assembly can't be loaded. Exception details: {1}", assembly.FullName, e.Message);
						_logger.Fatal(msg, e);
						throw new Exception(msg);
					}
				}

				foreach (var item in registrationContainer.ResolveAll<IComponentInstaller>())
					item.Install(container, assemblies);
			}
		}
	}
}