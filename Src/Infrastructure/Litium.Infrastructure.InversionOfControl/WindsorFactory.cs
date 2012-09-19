using System.Reflection;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using Litium.Common.InversionOfControl;

namespace Litium.Infrastructure.InversionOfControl
{
	public class WindsorFactory
	{
		public IIoCContainer Create(Assembly[] assamblies)
		{
			var container = new WindsorContainer();
			//container.Register(Component.For<IWindsorContainer>().Instance(container));
			container.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();

			return new IoCWindsorContainer(container, assamblies);
		}
	}
}