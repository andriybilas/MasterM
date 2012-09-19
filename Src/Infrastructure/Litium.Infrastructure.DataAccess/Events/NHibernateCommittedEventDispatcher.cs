using Litium.Common.Events;
using NHibernate.Event;

namespace Litium.Infrastructure.DataAccess.Events
{
	public class NHibernateCommittedEventDispatcher : ICommittedEventListener,
	                                                 IPostInsertEventListener,
	                                                 IPostUpdateEventListener,
	                                                 IPostDeleteEventListener
	{
		public void OnPostInsert(PostInsertEvent @event)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (InsertedEvent));
		}

		public void OnPostUpdate(PostUpdateEvent @event)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (UpdatedEvent));
		}

		public void OnPostDelete(PostDeleteEvent @event)
		{
			EventDispatcher.RaiseEvent(@event.Entity, typeof (DeletedEvent));
		}
	}
}