using System;
using System.Collections.Generic;

namespace Litium.Common.InversionOfControl
{
	public interface IIoCContainer
	{
		ComponentRegistration For<T>();
		ComponentRegistration For(Type type);
		object Resolve(Type type);

		T Resolve<T>();
		T Resolve<T>(bool throwExceptionIfNoComponent);
		T Resolve<T>(string key);
		IEnumerable<T> ResolveAll<T>();
		IEnumerable<object> ResolveAll(Type type);

		T ResolvePlugin<T>(string pluginName);
		T ResolvePlugin<T>(Type pluginType);
		T ResolvePlugin<T>(Type pluginType, bool throwExceptionIfNoComponent);
		T ResolvePlugin<T>(string pluginName, bool throwExceptionIfNoComponent);
	}
}