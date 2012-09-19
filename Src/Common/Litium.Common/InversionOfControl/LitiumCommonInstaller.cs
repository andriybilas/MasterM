using System;
using System.Reflection;
using Litium.Common.DataAccess;
using Litium.Common.Events;
using Litium.Common.Lifecycle;
using Litium.Common.Querying;

namespace Litium.Common.InversionOfControl
{
	public class LitiumCommonInstaller : IComponentInstaller
	{
		private readonly Type[] _typedServices = new[]
		                                         	{
		                                         		typeof (IEventListener<,>),
		                                         		typeof (IAsyncEventListener<,>),
		                                         		typeof (IConversation),
		                                         		typeof (IDataRepositoryManager),
		                                         		typeof (IDataCacheManager),
		                                         		typeof (GlobalRepository),
		                                         		typeof (IReleaseTask),
		                                         		typeof (ISetupTask),
		                                         	};

		void IComponentInstaller.Install(IIoCContainer container, Assembly[] assemblies)
		{
			foreach (var type in _typedServices)
				container.For(type).RegisterAsSingleton();

			container.For<IQueryProcessorFactory>().AsPlugin().RegisterAsTransient();
		}
	}
}