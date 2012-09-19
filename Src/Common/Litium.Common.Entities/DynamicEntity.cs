using System.Diagnostics.Contracts;
using Litium.Common.Entities.FieldFramework;

namespace Litium.Common.Entities
{
	public abstract class DynamicEntity : Entity
	{
		private FieldCollection _metadata = new FieldCollection();

		/// <summary>
		/// Collection of dynamic created properties.
		/// </summary>
		public virtual FieldCollection Fields
		{
			get
			{
				Contract.Ensures(Contract.Result<FieldCollection>() != null);
				Contract.Assume(_metadata != null);
				return _metadata;
			}
			protected internal set { _metadata = value; }
		}

		/// <summary>
		/// Dynamic Metadata property.  
		/// </summary>
		public virtual dynamic Metadata
		{
			get { return Fields; }
		}
	}
}