using System;
using System.Collections.Generic;
using System.Diagnostics;
using Litium.Common.InversionOfControl;

namespace Litium.Common
{
	/// <summary>
	/// 	Dependency injection container.
	/// </summary>
	[DebuggerStepThrough]
	public static class IoC
	{
		/// <summary>
		/// 	Gets the underlying IoC container.
		/// </summary>
		/// <value>The container.</value>
		public static IIoCContainer Container { get; internal set; }
		
		/// <summary>
		/// 	Releases unmanaged and - optionally - managed resources
		/// </summary>
		[DebuggerStepThrough]
		public static void Dispose()
		{
			//Container.Dispose();
			Container = null;
		}

		/// <summary>
		/// 	Returns a component instance by the service.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T Resolve<T>()
		{
			return Container.Resolve<T>();
		}

		/// <summary>
		/// Returns a component instance by the service.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T Resolve<T>(bool throwException)
		{
			return Container.Resolve<T>(throwException);
		}

		/// <summary>
		/// 	Resolve all valid components that match this type.
		/// </summary>
		/// <typeparam name = "T">The service type.</typeparam>
		/// <returns>The all component instance.</returns>
		[DebuggerStepThrough]
		public static IEnumerable<T> ResolveAll<T>()
		{
			return Container.ResolveAll<T>();
		}

		/// <summary>
		/// 	Resolve all valid components that match this type.
		/// </summary>
		/// <param name = "type">The type.</param>
		/// <returns>The all component instance.</returns>
		[DebuggerStepThrough]
		public static IEnumerable<object> ResolveAll(Type type)
		{
			return Container.ResolveAll(type);
		}

		/// <summary>
		/// 	Returns a component instance by the plugin name.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginName">Name of the plugin.</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T ResolvePlugin<T>(string pluginName)
		{
			return Container.ResolvePlugin<T>(pluginName);
		}

		/// <summary>
		/// 	Returns a component instance by the plugins full type.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginType">Type of the plugin.</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T ResolvePlugin<T>(Type pluginType)
		{
			return Container.ResolvePlugin<T>(pluginType);
		}

		/// <summary>
		/// Returns a component instance by the plugins full type.
		/// </summary>
		/// <typeparam name="T">Service type.</typeparam>
		/// <param name="pluginType">Type of the plugin.</param>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T ResolvePlugin<T>(Type pluginType, bool throwException)
		{
			return Container.ResolvePlugin<T>(pluginType, throwException);
		}

		/// <summary>
		/// Returns a component instance by the plugin name.
		/// </summary>
		/// <typeparam name="T">Service type.</typeparam>
		/// <param name="pluginName">Name of the plugin.</param>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public static T ResolvePlugin<T>(string pluginName, bool throwException)
		{
			return Container.ResolvePlugin<T>(pluginName, throwException);
		}

		/// <summary>
		/// Sets the conateiner.
		/// </summary>
		/// <param name="container">The container.</param>
		public static void SetConateiner(IIoCContainer container)
		{
			if (Container != null)
				throw new Exception("IoC engine is already initiated.");

			Container = container;
		}
	}
}