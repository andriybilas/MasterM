using System;
using System.Runtime.Serialization;
using Litium.Common.Entities;

namespace Litium.Common.Events
{
	public class EventHandlingException<TEntity, TEvent> : EventHandlingException<TEntity>
		where TEntity : Entity
		where TEvent : IEventType
	{
		public EventHandlingException(EntityEventArgs<TEntity> eventArgs)
			: base(typeof(TEvent), eventArgs)
		{
		}

		public EventHandlingException(string message, EntityEventArgs<TEntity> eventArgs)
			: base(message, typeof(TEvent), eventArgs)
		{
		}

		public EventHandlingException(string message, Exception innerException, EntityEventArgs<TEntity> eventArgs)
			: base(message, innerException, typeof(TEvent), eventArgs)
		{
		}

		protected EventHandlingException(SerializationInfo info, StreamingContext context, EntityEventArgs<TEntity> eventArgs)
			: base(info, context, typeof(TEvent), eventArgs)
		{
		}
	}

	public class EventHandlingException<TEntity> : EventHandlingException
		where TEntity : Entity
	{
		public EntityEventArgs<TEntity> EventArgs { get; set; }

		public EventHandlingException(Type eventType, EntityEventArgs<TEntity> eventArgs)
			: base(typeof(TEntity), eventType)
		{
			EventArgs = eventArgs;
		}

		public EventHandlingException(string message, Type eventType, EntityEventArgs<TEntity> eventArgs)
			: base(message, typeof(TEntity), eventType)
		{
			EventArgs = eventArgs;
		}

		public EventHandlingException(string message, Exception innerException, Type eventType, EntityEventArgs<TEntity> eventArgs)
			: base(message, innerException, typeof(TEntity), eventType)
		{
			EventArgs = eventArgs;
		}

		protected EventHandlingException(SerializationInfo info, StreamingContext context, Type eventType, EntityEventArgs<TEntity> eventArgs)
			: base(info, context, typeof(TEntity), eventType)
		{
			EventArgs = eventArgs;
		}
	}

	public class EventHandlingException : Exception
	{
		public Type EntityType { get; set; }
		public Type EventType { get; set; }

		public EventHandlingException(Type entityType, Type eventType)
		{
			EntityType = entityType;
			EventType = eventType;
		}

		public EventHandlingException(string message, Type entityType, Type eventType)
			: base(message)
		{
			EntityType = entityType;
			EventType = eventType;
		}

		public EventHandlingException(string message, Exception innerException, Type entityType, Type eventType)
			: base(message, innerException)
		{
			EntityType = entityType;
			EventType = eventType;
		}

		protected EventHandlingException(SerializationInfo info, StreamingContext context, Type entityType, Type eventType)
			: base(info, context)
		{
			EntityType = entityType;
			EventType = eventType;
		}
	}
}
