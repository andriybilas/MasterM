using System;
using Litium.Common.Entities;

namespace Litium.Common.Events
{
	public class EntityEventArgs<T> : EventArgs
		where T : Entity
	{
		public T Entity { get; set; }
		public Guid EntityId { get; set; }

		public EntityEventArgs()
		{
		}

		public EntityEventArgs(T entity)
		{
			Entity = entity;
			if (entity != null)
			{
				EntityId = entity.Id;
			}
		}

		public EntityEventArgs(Guid entityId)
		{
			EntityId = entityId;
		}
	}
}