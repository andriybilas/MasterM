using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Litium.Common.Entities;
using Litium.Common.Extensions;
using Litium.Common.WorkUnit;
using log4net;

namespace Litium.Common.Events
{
	public static class EventDispatcher
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Raises event, looks for listeners and invoke their event handling method
		/// </summary>
		/// <param name="entity">Entity that causes the event</param>
		/// <param name="eventType">Event type</param>
		public static void RaiseEvent(object entity, Type eventType)
		{
			//Raise generic entity type event
			Type genericEntityType = typeof(Entity);
			EntityEventArgs<Entity> genericArgs = entity as Entity != null
				? new EntityEventArgs<Entity>((Entity)entity)
				: new EntityEventArgs<Entity>();
			RaiseEvent(genericArgs, genericEntityType, eventType);

			//Raise specific entity type event
			Type entityType = GetEntityType(entity);
			Type eventArgsType = typeof(EntityEventArgs<>).MakeGenericType(entityType);
			var eventArgs = Activator.CreateInstance(eventArgsType, entity);
			RaiseEvent(eventArgs, entityType, eventType);
		}

		/// <summary>
		/// Raises event, looks for listeners and invoke their event handling method
		/// </summary>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventType">Event type</param>
		public static void RaiseEvent<TEntity>(EntityEventArgs<TEntity> eventArgs, Type eventType) where TEntity : Entity
		{
			//Raise specific entity type event
			Type entityType = typeof (TEntity);
			RaiseEvent(eventArgs, entityType, eventType);

			//Raise generic entity type event
			Type genericEntityType = typeof (Entity);
			EntityEventArgs<Entity> genericArgs = eventArgs.Entity != null 
				? new EntityEventArgs<Entity>(eventArgs.Entity) 
				: new EntityEventArgs<Entity>(eventArgs.EntityId);
			RaiseEvent(genericArgs, genericEntityType, eventType);
		}

		private static void RaiseEvent(object eventArgs, Type entityType, Type eventType)
		{
			bool requiresCommit = RequiresCommit(eventType);

			//Raise synchronous event
			RaiseEvent(eventArgs, entityType, eventType, requiresCommit, false);

			//Raise asynchronous event
			RaiseEvent(eventArgs, entityType, eventType, requiresCommit, true);
		}

		private static void RaiseEvent(object eventArgs, Type entityType, Type eventType, bool requiresCommit, bool async)
		{
			MethodInfo handleMethod = GetHandleEventMethod(entityType, eventType, async);
			Type listenerType = GetListenerType(entityType, eventType, async);
			var listeners = IoC.ResolveAll(listenerType);
			
			//Handle event listeners after unit of work is committed
			if (requiresCommit)
			{
				foreach (var listener in listeners)
				{
					RegisterCommitedEventHandler(listener, handleMethod, eventArgs, entityType, eventType, async);
				}
			}
			//Handle event listeners immediately
			else
			{
				foreach (var listener in listeners)
				{
					HandleEvent(listener, handleMethod, eventArgs, entityType, eventType, async);
				}
			}
		}

		private static void HandleEvent(object handler, MethodInfo method, object args, Type entityType, Type eventType, bool async)
		{
			if(async)
			{
				HandleEventAsynchronously(handler, method, args, entityType, eventType);
			}
			else
			{
				HandleEventSynchronously(handler, method, args, entityType, eventType);
			}
		}

		private static void HandleEventSynchronously(object handler, MethodInfo method, object args, Type entityType, Type eventType)
		{
			try
			{
				method.Invoke(handler, new[] { args });
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		private static void HandleEventAsynchronously(object handler, MethodInfo method, object args, Type entityType, Type eventType)
		{
			var action = new Action(() =>
			{
				try
				{
					_log.InfoFormat("Handling '{0}' event for '{1}' entity by '{2} -> {3}' began.", eventType.Name, entityType.Name, handler.GetType().Name, method.Name);

					method.Invoke(handler, new[] { args });

					_log.InfoFormat("Handling '{0}' event for '{1}' entity by '{2} -> {3}' finished.", eventType.Name, entityType.Name, handler.GetType().Name, method.Name);
				}
				catch (TargetInvocationException exception)
				{
					_log.Error(string.Format("Handling '{0}' event for '{1}' entity by '{2} -> {3}' failed.", eventType.Name, entityType.Name, handler.GetType().Name, method.Name), exception.InnerException);
				}
				catch (Exception exception)
				{
					_log.Error(string.Format("Handling '{0}' event for '{1}' entity by '{2} -> {3}' failed.", eventType.Name, entityType.Name, handler.GetType().Name, method.Name), exception);
				}
			});

			var task = new Task(action);
			task.Start();
		}

		private static void RegisterCommitedEventHandler(object handler, MethodInfo method, object args, Type entityType, Type eventType, bool async)
		{
			var unitOfWork = UnitOfWork.Current;
			if (unitOfWork == null) return;

			unitOfWork.PostCommitActions.Add(() => HandleEvent(handler, method, args, entityType, eventType, async));
		}

		private static Type GetListenerType(Type entityType, Type eventType, bool async)
		{
			Type listenerType = async ? typeof(IAsyncEventListener<,>) : typeof(IEventListener<,>);
			return listenerType.MakeGenericType(new[] {entityType, eventType});
		}

		private static MethodInfo GetHandleEventMethod(Type entityType, Type eventType, bool async)
		{
			var reflection = new object();
			MethodInfo handleMethod = async
				? reflection.GetMethodInfo<IAsyncEventListener<Entity, IEventType>>(x => x.HandleEvent(null))
				: reflection.GetMethodInfo<IEventListener<Entity, IEventType>>(x => x.HandleEvent(null));
			return handleMethod.Convert(new[] {entityType, eventType});
		}

		private static bool RequiresCommit(Type eventType)
		{
			return (eventType.GetInterfaces().Contains(typeof (ICommitedEventType)));
		}

		private static Type GetEntityType(object entity)
		{
			Type entityType = entity.GetType();
			while (entityType != null && entityType.Name.EndsWith("Proxy"))
			{
				entityType = entityType.BaseType;
			}
			return entityType;
		}
	}
}