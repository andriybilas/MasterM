using System;

namespace Litium.Common.InversionOfControl
{
	public interface IComponentRegistrationProcessor
	{
		void AsPlugin();
		void RegisterAsSingleton(Type type);
		void RegisterAsTransient(Type type);
		void RegisterInstance(Type type, object instance);
	}
}