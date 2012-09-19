using System;

namespace Litium.Common.InversionOfControl
{
	/// <summary>
	/// 	Plugin description.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class PluginAttribute : Attribute
	{
		/// <summary>
		/// 	Initializes a new instance of the <see cref = "PluginAttribute" /> class.
		/// </summary>
		/// <param name = "name">The name.</param>
		public PluginAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// 	Gets the name of the plugin.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }
	}
}