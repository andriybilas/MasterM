using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using Castle.MicroKernel;
using Castle.Windsor;
using Litium.Common.InversionOfControl;
using log4net;

namespace Litium.Infrastructure.InversionOfControl
{
	/// <summary>
	/// Windsor container implementation
	/// </summary>
	public class IoCWindsorContainer : IIoCContainer
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(IoCWindsorContainer));
		private readonly Assembly[] _assamblies;
		private readonly IWindsorContainer _container;
		private readonly bool _isDebugMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="IoCWindsorContainer"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="assamblies">The assamblies.</param>
		public IoCWindsorContainer(IWindsorContainer container, Assembly[] assamblies)
		{
			_isDebugMode = ((CompilationSection)ConfigurationManager.GetSection("system.web/compilation")).Debug;
			_container = container;
			_assamblies = assamblies;
		}

		/// <summary>
		/// Register component for <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type.</typeparam>
		/// <returns>
		/// 	<see cref="ComponentRegistration"/> to register the component in ioc container.
		/// </returns>
		public ComponentRegistration For<T>()
		{
			return new ComponentRegistration(new WindosorComponentRegistration(_container, _assamblies), typeof(T));
		}

		/// <summary>
		/// Register component for <param name="type"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<see cref="ComponentRegistration"/> to register the component in ioc container.
		/// </returns>
		public ComponentRegistration For(Type type)
		{
			return new ComponentRegistration(new WindosorComponentRegistration(_container, _assamblies), type);
		}

		/// <summary>
		/// Resolves the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public object Resolve(Type type)
		{
			return Resolve(type, true);
		}

		/// <summary>
		/// Resolves the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public object Resolve(Type type, bool throwException)
		{
			try
			{
				return _container.Resolve(type);
			}
			catch (ComponentNotFoundException)
			{
				if (throwException)
					throw;
			}
			catch (Exception e)
			{
				_log.Error(e.Message, e);
				if (_isDebugMode || throwException)
					throw;
			}
			return null;

		}
		/// <summary>
		/// 	Returns a component instance by the service.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "throwException">if set to <c>true</c> [throw exception if no component].</param>
		/// <returns>The component instance.</returns>
		/// <exception cref = "ComponentNotFoundException">is thrown when the component not is found.</exception>
		/// <exception cref = "Exception">is thrown when exception occured during creation. The exception will throws if web-site running in debug mode or <paramref name="throwException" /> is <c>true</c>.</exception>
		[DebuggerStepThrough]
		public T Resolve<T>(bool throwException)
		{
			try
			{
				return _container.Resolve<T>();
			}
			catch (ComponentNotFoundException)
			{
				if (throwException)
					throw;
			}
			catch (Exception e)
			{
				_log.Error(e.Message, e);
				if (_isDebugMode || throwException)
					throw;
			}
			return default(T);
		}


		/// <summary>
		/// 	Returns a component instance by the service.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <exception cref = "ComponentNotFoundException">is thrown when the component not is found.</exception>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public T Resolve<T>()
		{
			return Resolve<T>(true);
		}

		/// <summary>
		/// Resolves the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public T Resolve<T>(string key)
		{
			return Resolve<T>(key, true);
		}
		/// <summary>
		/// Resolves the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public T Resolve<T>(string key, bool throwException)
		{
			try
			{
				return _container.Resolve<T>(key);
			}
			catch (ComponentNotFoundException)
			{
				if (throwException)
					throw;
			}
			catch (Exception e)
			{
				_log.Error(e.Message, e);
				if (_isDebugMode || throwException)
					throw;
			}
			return default(T);
		}

		/// <summary>
		/// 	Resolve all valid components that match this type.
		/// </summary>
		/// <typeparam name = "T">The service type.</typeparam>
		/// <returns>The all component instance.</returns>
		[DebuggerStepThrough]
		public IEnumerable<T> ResolveAll<T>()
		{
			return ResolveAll<T>(true);
		}

		/// <summary>
		/// Resolves all.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public T[] ResolveAll<T>(bool throwException)
		{
			try
			{
				return _container.ResolveAll<T>();
			}
			catch (Exception e)
			{
				_log.Error(e.Message, e);
				if (throwException)
					throw;
			}
			return default(T[]);
		}

		/// <summary>
		/// 	Resolve all valid components that match this type.
		/// </summary>
		/// <param name = "type">The type.</param>
		/// <returns>The all component instance.</returns>
		[DebuggerStepThrough]
		public IEnumerable<object> ResolveAll(Type type)
		{
			var items = _container.ResolveAll(type);
			return items.Cast<object>();
		}

		/// <summary>
		/// 	Returns a component instance by the plugin name.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginName">Name of the plugin.</param>
		/// <exception cref = "ComponentNotFoundException">is thrown when the component not is found.</exception>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public T ResolvePlugin<T>(string pluginName)
		{
			return _container.Resolve<T>(_container.GetPluginName(typeof(T), pluginName));
		}

		/// <summary>
		/// 	Returns a component instance by the plugins full type.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginType">Type of the plugin.</param>
		/// <returns>The component instance.</returns>
		/// <exception cref = "ComponentNotFoundException">is thrown when the component not is found.</exception>
		[DebuggerStepThrough]
		public T ResolvePlugin<T>(Type pluginType)
		{
			return ResolvePlugin<T>(pluginType.FullName, true);
		}

		/// <summary>
		/// 	Returns a component instance by the plugins full type.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginType">Type of the plugin.</param>
		/// <param name = "throwExceptionIfNoComponent">if set to <c>true</c> [throw exception if no component].</param>
		/// <returns>The component instance.</returns>
		[DebuggerStepThrough]
		public T ResolvePlugin<T>(Type pluginType, bool throwExceptionIfNoComponent)
		{
			return ResolvePlugin<T>(pluginType.FullName, throwExceptionIfNoComponent);
		}

		/// <summary>
		/// 	Returns a component instance by the plugin name.
		/// </summary>
		/// <typeparam name = "T">Service type.</typeparam>
		/// <param name = "pluginName">Name of the plugin.</param>
		/// <param name = "throwExceptionIfNoComponent">if set to <c>true</c> [throw exception if no component].</param>
		/// <returns>The component instance.</returns>
		/// <exception cref = "ComponentNotFoundException">is thrown when the component not is found.</exception>
		[DebuggerStepThrough]
		public T ResolvePlugin<T>(string pluginName, bool throwExceptionIfNoComponent)
		{
			return Resolve<T>(_container.GetPluginName(typeof(T), pluginName), throwExceptionIfNoComponent);
		}

		/// <summary>
		/// Gets the registrated components.
		/// </summary>
		/// <value>The registrated components.</value>
		public IOrderedEnumerable<KeyValuePair<string, string>> RegistratedComponents
		{
			get
			{
				var registeredTypes = _container.Kernel.GetAssignableHandlers(typeof(object)).Select(handler =>
				{
					var name = handler.ComponentModel.Name.Equals(handler.ComponentModel.Implementation.FullName)
						? null
						: handler.ComponentModel.Name.Substring(handler.ComponentModel.Service.FullName.Length).TrimStart('(').TrimEnd(')');
					return new KeyValuePair<string, string>(
						string.Format("{0}{1}", handler.ComponentModel.Service, string.IsNullOrEmpty(name) ? string.Empty : string.Format("({0})", name)),
						handler.ComponentModel.Implementation.ToString());
				});
				return registeredTypes.OrderBy(a => a.Key);
			}
		}

		/// <summary>
		/// 	Releases the specified instance.
		/// </summary>
		/// <param name = "instance">The instance.</param>
		[DebuggerStepThrough]
		public void Release(object instance)
		{
			_container.Release(instance);
		}
	}
}