using System;
using System.Reflection;
using Litium.Common.InversionOfControl;
using Litium.Domain.Services;

namespace Litium.Domain.Lifecycle
{
	/// <summary>
	/// Litium domain library installer. Register all interfaces that need to resolve.
	/// </summary>
	internal class LitiumDomainInstaller : IComponentInstaller
	{
		private readonly Type[] _serviceTypes = new[]
		                                        	{
		                                        		typeof (IMediaArchiveService)
		                                        		//typeof (IPublishingService)
		                                        	};

		public void Install(IIoCContainer container, Assembly[] assemblies)
		{
			foreach (Type service in _serviceTypes)
				container.For(service).RegisterAsTransient();
		}
	}
}