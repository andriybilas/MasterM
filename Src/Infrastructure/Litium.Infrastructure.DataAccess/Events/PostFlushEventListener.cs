using System;
using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace Litium.Infrastructure.DataAccess.Events
{
	// Workaround
	// https://groups.google.com/group/nhusers/browse_thread/thread/a886504d871a7cc/2b65f0d6df7fe77b
	[Serializable]
	internal class PostFlushEventListener : DefaultFlushEventListener
	{
		public override void OnFlush(FlushEvent @event)
		{
			try
			{
				base.OnFlush(@event);
			}
			catch (AssertionFailure)
			{
			}
		}
	}
}