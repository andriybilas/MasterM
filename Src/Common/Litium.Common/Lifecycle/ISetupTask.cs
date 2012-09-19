using System.Reflection;

namespace Litium.Common.Lifecycle
{
	public interface ISetupTask
	{
		void Setup(Assembly[] assemblies);
	}
}