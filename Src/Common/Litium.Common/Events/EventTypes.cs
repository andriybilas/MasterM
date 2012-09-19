namespace Litium.Common.Events
{
	public interface IEventType
	{
	}

	public interface ICommitedEventType : IEventType
	{
	}

	public class ValidateEvent : IEventType
	{
	}

	public class InsertEvent : IEventType
	{
	}

	public class InsertedEvent : ICommitedEventType
	{
	}

	public class UpdateEvent : IEventType
	{
	}

	public class UpdatedEvent : ICommitedEventType
	{
	}

	public class DeleteEvent : IEventType
	{
	}

	public class DeletedEvent : ICommitedEventType
	{
	}

	internal class LoadedEvent : IEventType
	{
	}

	//These are extra events which are not currently used but could be usefull in future
	//TODO: Remove if isn't used

	//public class LoadingEvent : IEventType
	//{
	//}

	//public class InsertingEvent : IEventType
	//{
	//}

	//public class InsertedPreCommitEvent : IEventType
	//{
	//}

	//public class UpdatingEvent : IEventType
	//{
	//}

	//public class UpdatedPreCommitEvent : IEventType
	//{
	//}

	//public class DeletingEvent : IEventType
	//{
	//}

	//public class DeletedPreCommitEvent : IEventType
	//{
	//}

	//public class MergeEvent : IEventType
	//{
	//}
}
