using System;

namespace Litium.Common.Entities
{
	/// <summary>
	/// 	Base class for all entities.
	/// </summary>
    public abstract class Entity : BaseObject<Guid>, ICloneable
	{
		/// <summary>
		/// 	Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns> A new object that is a copy of this instance. </returns>
		public virtual object Clone()
		{
			var entity = (Entity) MemberwiseClone();
			entity.Id = Guid.Empty;
			return entity;
		}

		public abstract object ValidationCopy();
	}
}