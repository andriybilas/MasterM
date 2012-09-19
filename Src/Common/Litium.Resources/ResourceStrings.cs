using System;
using System.Reflection;
using System.Resources;

namespace Litium.Resources
{
	public abstract class ResourceStrings<T> where T : class
	{
		protected static ResourceManager _manager;

		protected ResourceStrings (String resourceAssemblyName)
		{
			Assembly assembly = typeof (T).Assembly;
			_manager = new ResourceManager (resourceAssemblyName, assembly);
		}
	}
}