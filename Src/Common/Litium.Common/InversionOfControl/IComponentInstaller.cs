using System.Reflection;

namespace Litium.Common.InversionOfControl
{
	/// <summary>
	/// 	Dependency injector contaner installer.
	/// </summary>
	public interface IComponentInstaller
	{
		void Install(IIoCContainer container, Assembly[] assemblies);
	}
}