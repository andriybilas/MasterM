using System;
using System.Collections;
using Iesi.Collections;
using Litium.Common.Entities;
using Litium.Common.Events;
using NHibernate.Event;
using DeleteEvent = NHibernate.Event.DeleteEvent;

namespace Litium.Infrastructure.DataAccess.Events
{
	public class NHibernateEventDispatcher : IEventListener,
	                                         IPostLoadEventListener,
	                                         ISaveOrUpdateEventListener,
	                                         IDeleteEventListener,
	                                         IMergeEventListener
		//These are extra events which are not currently used but could be usefull in future
		//TODO: Remove if isn't used
		//IPreInsertEventListener,
		//IPreUpdateEventListener,
		//IPreLoadEventListener,
		//IPostInsertEventListener,
		//IPostUpdateEventListener,
		//IPreDeleteEventListener,
		//IPostDeleteEventListener,
	{
		public void OnPostLoad(PostLoadEvent @event)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (LoadedEvent));
		}

		public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
		{
			var entity = @event.Entity as Entity;
			if (entity == null) return;

			EventDispatcher.RaiseEvent(@event.Entity, entity.Id.Equals(Guid.Empty) ? typeof (InsertEvent) : typeof (UpdateEvent));
			
			//Validate event is raised here instead of on PreInsert/PreUpdate events to make our validation before the
			//NHibernate DefaultSaveOrUpdateEventListener is handled since it throws own exceptions if entity isn't valid
			//according to its Map.
			EventDispatcher.RaiseEvent(@event.Entity, typeof (ValidateEvent));
		}

		public void OnDelete(DeleteEvent @event)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (Common.Events.DeleteEvent));
		}

		public void OnDelete(DeleteEvent @event, ISet transientEntities)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (Common.Events.DeleteEvent));
		}

		public void OnMerge(MergeEvent @event)
		{
			if (@event.Entity == null)
				return;

			EventDispatcher.RaiseEvent(@event.Entity, typeof (UpdateEvent));

			//Validate event is raised here instead of on PreInsert/PreUpdate events to make our validation before the
			//NHibernate DefaultSaveOrUpdateEventListener is handled since it throws own exceptions if entity isn't valid
			//according to its Map.
			EventDispatcher.RaiseEvent(@event.Entity, typeof(ValidateEvent));
		}

		public void OnMerge(MergeEvent @event, IDictionary copiedAlready)
		{
			if (@event.Entity == null)
				return;

			EventDispatcher.RaiseEvent(@event.Entity, typeof (UpdateEvent));

			//Validate event is raised here instead of on PreInsert/PreUpdate events to make our validation before the
			//NHibernate DefaultSaveOrUpdateEventListener is handled since it throws own exceptions if entity isn't valid
			//according to its Map.
			EventDispatcher.RaiseEvent(@event.Entity, typeof(ValidateEvent));
		}

		//These are extra events which are not currently used but could be usefull in future
		//TODO: Remove if isn't used

		//public bool OnPreInsert(PreInsertEvent @event)
		//{
		//    //TODO: Remove if isn't used
		//    //EventDispatcher.RaiseEvent(@event.Entity, typeof(InsertingEvent));

		//    PerformInNoFlushWhenFindMode(() =>
		//                                 EventDispatcher.RaiseEvent(@event.Entity, typeof (ValidateEvent)),
		//                                 @event);
		//    return false;
		//}

		//public bool OnPreUpdate(PreUpdateEvent @event)
		//{
		//    //TODO: Remove if isn't used
		//    //EventDispatcher.RaiseEvent(@event.Entity, typeof(UpdatingEvent));

		//    PerformInNoFlushWhenFindMode(() =>
		//                                 EventDispatcher.RaiseEvent(@event.Entity, typeof (ValidateEvent)), 
		//                                 @event);
		//    return false;
		//}

		//public void OnPreLoad(PreLoadEvent @event)
		//{
		//    EventDispatcher.RaiseEvent(@event.Entity, typeof (LoadingEvent));
		//}

		//public void OnPostInsert(PostInsertEvent @event)
		//{
		//    EventDispatcher.RaiseEvent(@event.Entity, typeof (InsertedPreCommitEvent));
		//}

		//public void OnPostUpdate(PostUpdateEvent @event)
		//{
		//    EventDispatcher.RaiseEvent(@event.Entity, typeof (UpdatedPreCommitEvent));
		//}

		//public bool OnPreDelete(PreDeleteEvent @event)
		//{
		//    EventDispatcher.RaiseEvent(@event.Entity, typeof (DeletingEvent));
		//    return false;
		//}

		//public void OnPostDelete(PostDeleteEvent @event)
		//{
		//    EventDispatcher.RaiseEvent(@event.Entity, typeof (DeletedPreCommitEvent));
		//}

		///// <summary>
		///// This method is used to prevent cyclic PreInstert/PreUpdate events calling.
		///// When Querying is called inside these events it causes Flush method to be called which causes rasing the PreInsert/PreUpdate event and makes cycle.
		///// </summary>
		//private void PerformInNoFlushWhenFindMode(Action action, IPreDatabaseOperationEventArgs @event)
		//{
		//    var sessionImplementation = @event.Session.GetSessionImplementation();
		//    var originalFlushMode = sessionImplementation.FlushMode;
		//    if (originalFlushMode != FlushMode.Always && originalFlushMode != FlushMode.Auto)
		//    {
		//        action.Invoke();
		//    }
		//    else
		//    {
		//        sessionImplementation.FlushMode = FlushMode.Commit; //no Flush when Querying
		//        try
		//        {
		//            action.Invoke();
		//        }
		//        finally
		//        {
		//            sessionImplementation.FlushMode = originalFlushMode; //return original mode
		//        }
		//    }
		//}
	}
}