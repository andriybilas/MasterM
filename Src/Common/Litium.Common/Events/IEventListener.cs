using Litium.Common.Entities;

namespace Litium.Common.Events
{
	/// <summary>
	/// All synchronous event listeners should be derived from this interface defining type of entity that causes event and type of event
	/// </summary>
	/// <typeparam name="TEntity">Type of entity that causes the event</typeparam>
	/// <typeparam name="TEvent">Type of event that should be derived from <see cref="IEventType"/></typeparam>
	public interface IEventListener<TEntity, TEvent>
		where TEntity : Entity
		where TEvent : IEventType
	{
		/// <summary>
		/// Performs event handling
		/// </summary>
		/// <param name="eventArgs">Event arguments</param>
		void HandleEvent(EntityEventArgs<TEntity> eventArgs);
	}
}