using System;

namespace Litium.Common.InversionOfControl
{
	public class ComponentRegistration : LifeStyleComponentRegistration
	{
		private readonly IComponentRegistrationProcessor _registrationProcessor;

		public ComponentRegistration(IComponentRegistrationProcessor registrationProcessor, Type type)
			: base(registrationProcessor, type)
		{
			_registrationProcessor = registrationProcessor;
		}

		public LifeStyleComponentRegistration AsPlugin()
		{
			_registrationProcessor.AsPlugin();
			return this;
		}

		public void RegisterInstance(object instance)
		{
			_registrationProcessor.RegisterInstance(Type, instance);
		}
	}
}