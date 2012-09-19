using System;

namespace Litium.Common.InversionOfControl
{
	public class LifeStyleComponentRegistration
	{
		private readonly IComponentRegistrationProcessor _registrationProcessor;

		public LifeStyleComponentRegistration(IComponentRegistrationProcessor registrationProcessor, Type type)
		{
			Type = type;
			_registrationProcessor = registrationProcessor;
		}

		public Type Type { get; private set; }

		public void RegisterAsSingleton()
		{
			_registrationProcessor.RegisterAsSingleton(Type);
		}

		public void RegisterAsTransient()
		{
			_registrationProcessor.RegisterAsTransient(Type);
		}
	}
}