using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Litium.Common.InversionOfControl;

namespace Litium.Infrastructure.InversionOfControl
{
	/// <summary>
	/// Windsor component registration
	/// </summary>
	public class WindosorComponentRegistration : IComponentRegistrationProcessor
	{
		private readonly IEnumerable<Assembly> _assemblies;
		private readonly IWindsorContainer _container;
		private bool _asPlugin;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindosorComponentRegistration"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="assemblies">The assemblies.</param>
		public WindosorComponentRegistration(IWindsorContainer container, IEnumerable<Assembly> assemblies)
		{
			_container = container;
			_assemblies = assemblies;
		}

		/// <summary>
		/// Register component as plugin.
		/// </summary>
		public void AsPlugin()
		{
			_asPlugin = true;
		}

		/// <summary>
		/// Registers as singleton.
		/// </summary>
		/// <param name="type">The type.</param>
		public void RegisterAsSingleton(Type type)
		{
			foreach (var assembly in _assemblies)
			{
				try
				{
					var item = AllTypes.FromAssembly(assembly)
						.IncludeNonPublicTypes()
						.BasedOn(type)
						.WithService.AllInterfaces();

					if (_asPlugin)
					{
						item = item.ConfigurePlugin(type);
					}

					_container.Register(item.Configure(c => c.LifeStyle.Singleton));
				}
				catch (Exception ex)
				{
					throw new ApplicationException(string.Format("Could not register {0} from assembly {1}. This can be due to not using the latest or incompatible {1} with current Litium Studio version.", type.FullName, assembly.FullName), ex);
				}
			}
		}

		/// <summary>
		/// Registers as transient.
		/// </summary>
		/// <param name="type">The type.</param>
		public void RegisterAsTransient(Type type)
		{
			foreach (var assembly in _assemblies)
			{
				try
				{
					var item = AllTypes.FromAssembly(assembly)
						.IncludeNonPublicTypes()
						.BasedOn(type)
						.WithService.AllInterfaces();

					if (_asPlugin)
					{
						item = item.ConfigurePlugin(type);
					}

					_container.Register(item.Configure(c => c.LifeStyle.Transient));
				}
				catch (Exception ex)
				{
					throw new ApplicationException(string.Format("Could not register {0} from assembly {1}. This can be due to not using the latest or incompatible {1} with current Litium Studio version.", type.FullName, assembly.FullName), ex);
				}
			}
		}

		/// <summary>
		/// Registers the instance.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="instance">The instance.</param>
		public void RegisterInstance(Type type, object instance)
		{
			_container.Register(Component.For(type).Instance(instance));
		}
	}
}