using System;
using System.Linq;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Litium.Common.InversionOfControl;

namespace Litium.Infrastructure.InversionOfControl
{
	/// <summary>
	/// 	Container extension
	/// </summary>
	public static class WindsorExtension
	{
		/// <summary>
		/// 	Register service based on the service as a service with the same name.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "fromAssemblyDescriptor">From assembly descriptor.</param>
		/// <returns></returns>
		public static BasedOnDescriptor BasedOnAsService<T>(this FromAssemblyDescriptor fromAssemblyDescriptor)
		{
			return fromAssemblyDescriptor.BasedOnAsService(typeof (T));
		}

		/// <summary>
		/// 	Register service based on the service as a service with the same name.
		/// </summary>
		/// <param name = "fromAssemblyDescriptor">From assembly descriptor.</param>
		/// <param name = "type">The type.</param>
		/// <returns></returns>
		public static BasedOnDescriptor BasedOnAsService(this FromAssemblyDescriptor fromAssemblyDescriptor, Type type)
		{
			return fromAssemblyDescriptor.BasedOn(type).WithService.Select(new[] {type});
		}

		/// <summary>
		/// 	Configures the life style.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <param name = "lifestyleType">Type of the lifestyle.</param>
		/// <returns></returns>
		public static BasedOnDescriptor ConfigureLifeStyle(this BasedOnDescriptor basedOnDescriptor, LifestyleType lifestyleType)
		{
			if (basedOnDescriptor == null) throw new ArgumentNullException("basedOnDescriptor");

			basedOnDescriptor
				.Configure(configurer => { configurer.LifeStyle.Is(lifestyleType); });

			return basedOnDescriptor;
		}

		/// <summary>
		/// 	Configures the plugin.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <param name = "lifestyleType">Type of the lifestyle.</param>
		/// <param name = "service">The service.</param>
		/// <returns></returns>
		public static BasedOnDescriptor ConfigurePlugin(this BasedOnDescriptor basedOnDescriptor, LifestyleType lifestyleType,
		                                                Type service)
		{
			if (basedOnDescriptor == null) throw new ArgumentNullException("basedOnDescriptor");

			return basedOnDescriptor.ConfigurePlugin(service).ConfigureLifeStyle(lifestyleType);
		}

		/// <summary>
		/// 	Configures the plugin.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <returns></returns>
		public static BasedOnDescriptor ConfigurePlugin<TService>(this BasedOnDescriptor basedOnDescriptor)
		{
			return ConfigurePlugin(basedOnDescriptor, typeof (TService));
		}

		/// <summary>
		/// 	Configures the plugin.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <param name = "service">The service.</param>
		/// <returns></returns>
		public static BasedOnDescriptor ConfigurePlugin(this BasedOnDescriptor basedOnDescriptor, Type service)
		{
			if (basedOnDescriptor == null) throw new ArgumentNullException("basedOnDescriptor");

			basedOnDescriptor
				.Configure(configurer =>
				           	{
				           		var attributes = configurer.Implementation.GetCustomAttributes(true);

				           		string name = attributes
				           			.OfType<PluginAttribute>()
				           			.Select(controllerAttribute => controllerAttribute.Name)
				           			.FirstOrDefault();

				           		if (name == null)
				           			return;

				           		configurer.Named(GetPluginName(null, service, name));
				           	});

			return basedOnDescriptor;
		}

		/// <summary>
		/// 	Configures the plugin with the service full name.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <returns></returns>
		public static BasedOnDescriptor ConfigurePluginAsServiceType(this BasedOnDescriptor basedOnDescriptor)
		{
			if (basedOnDescriptor == null) throw new ArgumentNullException("basedOnDescriptor");

			basedOnDescriptor
				.Configure(configurer => { configurer.Named(GetPluginName(null, configurer.ServiceType, configurer.Implementation.FullName)); });

			return basedOnDescriptor;
		}

		/// <summary>
		/// 	Gets the name of the plugin.
		/// </summary>
		/// <param name = "container">The container.</param>
		/// <param name = "service">The service.</param>
		/// <param name = "name">The name.</param>
		/// <returns></returns>
		public static string GetPluginName(this IWindsorContainer container, Type service, string name)
		{
			return (service.FullName + "(" + name + ")").ToLowerInvariant();
		}


		/// <summary>
		/// 	If the plugin exists.
		/// </summary>
		/// <param name = "basedOnDescriptor">The based on descriptor.</param>
		/// <returns></returns>
		public static BasedOnDescriptor IfPluginExists(this BasedOnDescriptor basedOnDescriptor)
		{
			if (basedOnDescriptor == null) throw new ArgumentNullException("basedOnDescriptor");

			basedOnDescriptor.If(type => type.GetCustomAttributes(typeof (PluginAttribute), true).Any());

			return basedOnDescriptor;
		}
	}
}