using System;
using System.Threading;
using System.Transactions;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Lifecycle;
using Litium.Common.WorkUnit;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using NHibernate;
using Xunit;

namespace Litium.Test.Common.Xunit.Events
{
	public class EventsTest : ConversationalTestBase
	{
		private static string _message1;
		private static string _message2;
		private static string _message3;

		[Fact]
		public void ChangeNameInEvent()
		{
			var item = new SimpleEventEntity
			           	{
			           		Name = "ChangeMe #1"
			           	};
			Repository.Data.Save(item);
			Assert.Equal("ChangeMe #1 create created", item.Name);

			ConversationHelper.ReOpen();

			var item2 = Repository.Data.Get<SimpleEventEntity>(item.Id);
			Assert.Equal("ChangeMe #1 create", item2.Name);
		}

		[Fact]
		public void ChangeNameInEventCheckInsertAndUpdateCount()
		{
			var statistics = IoC.Resolve<ISessionFactory>().Statistics;
			statistics.Clear();

			var item = new SimpleEventEntity
			           	{
			           		Name = "ChangeMe #1"
			           	};
			Repository.Data.Save(item);
			Assert.Equal("ChangeMe #1 create created", item.Name);

			Assert.Equal(1, statistics.EntityInsertCount);
			Assert.Equal(0, statistics.EntityUpdateCount);
		}

		[Fact]
		public void CustomizationTest()
		{
			var entity = new SimpleEntity {Name = "CustomizedSimpleEntity"};
			CustomService.Execute(entity);
			Assert.Equal("Custom event was handled", _message3);
		}

		[Fact]
		public void EventHandlingExceptionTest()
		{
			bool exceptionWasThrown;
			var entity = new SimpleEntity {Name = "EventHandlingExceptionTester"};
			try
			{
				SimpleEntity entity1 = entity;
				var exception = Assert.Throws(
					typeof (EventHandlingException<SimpleEntity, ValidateEvent>),
					() => Repository.Data.Save(entity1));
				throw exception;
			}
			catch (EventHandlingException<SimpleEntity, ValidateEvent> ex)
			{
				exceptionWasThrown = true;

				Assert.Equal(typeof (SimpleEntity), ex.EntityType);
				Assert.Equal(typeof (ValidateEvent), ex.EventType);
				Assert.True(ex.EventArgs.Entity != null);

				Console.WriteLine(
					string.Format("An exception was thrown during handling the '{0}' event for '{1}' entity with Id={2}.",
					              ex.EventType.Name, ex.EntityType.Name, ex.EventArgs.Entity.Id));
				Console.WriteLine("Message:");
				Console.WriteLine(ex.Message);
			}

			Assert.True(exceptionWasThrown);

			Repository.Data.Cache.Clear();
			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Null(entity);
		}

		[Fact]
		public void EventJobListenerTest()
		{
			using (var unitOfWork = new UnitOfWork())
			{
				var entity = new SimpleEntity
				             	{
				             		Name = "EventJobListenerTester",
				             	};
				Repository.Data.Save(entity);
				unitOfWork.Commit();
			}

			Thread.Sleep(2*1000);
			Assert.Equal("Job was performed", _message2);
		}

		[Fact]
		public void FullCycleEventsHandlingTest()
		{
			var entity = new SimpleEntity {Name = "FullCycleEventsHandlingTester"};
			Repository.Data.Save(entity);
			Assert.Equal("->Insert->Inserted", _message1);

			Repository.Data.Cache.Clear(entity);
			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Equal("->Insert->Inserted->Loaded", _message1);

			entity.Column1 += "Changed";
			Repository.Data.Save(entity);
			Assert.Equal(
				"->Insert->Inserted->Loaded->Update->Updated",
				_message1);

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity.Column2 += "Deleted";
			Repository.Data.Delete(entity);
			Assert.Equal(
				"->Insert->Inserted->Loaded->Update->Updated->Delete->Deleted",
				_message1);
		}

		[Fact]
		public void MultipleEventsInSameClass()
		{
			var item = new SimpleEventEntity
			           	{
			           		Name = "PageType #1"
			           	};

			SimpleEntityMultiEventListener.Counter = 0;
			Repository.Data.Save(item);
			Assert.Equal(110, SimpleEntityMultiEventListener.Counter);
		}

		//Commented due to the wrong behaviour expected (below is corrected test)
		//[Fact]
		//public void LongRunningPreAsyncEvent()
		//{
		//    var item = new SimpleEventEntity
		//    {
		//        Name = "AsyncCreated should be aborted"
		//    };
		//    Assert.Throws<TransactionManagerCommunicationException>(() => Repository.Data.Save(item));

		//    ConversationHelper.ReOpen();

		//    var item2 = Repository.Data.Get<SimpleEventEntity>(item.Id);
		//    Assert.Null(item2);
		//}

		[Fact]
		public void LongRunningPreAsyncEvent()
		{
			var item = new SimpleEventEntity
			{
				Name = "AsyncCreated should not be aborted since event is handled asynchronously"
			};
			Repository.Data.Save(item);

			ConversationHelper.ReOpen();

			var item2 = Repository.Data.Get<SimpleEventEntity>(item.Id);
			Assert.NotNull(item2);
		}

		[Fact]
		public void LongRunningPreEvent()
		{
			var item = new SimpleEventEntity
			{
				Name = "Created should be aborted"
			};
			Assert.Throws<TransactionManagerCommunicationException>(() => Repository.Data.Save(item));

			ConversationHelper.ReOpen();

			var item2 = Repository.Data.Get<SimpleEventEntity>(item.Id);
			Assert.Null(item2);
		}

		#region SimpleEntityEventJobListeners

		public class SimpleEntityInsertedAsyncEventListener1 : IAsyncEventListener<SimpleEntity, InsertedEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "EventJobListenerTester") return;
				Thread.Sleep(1*1000);
				_message2 = "Job was performed";
			}
		}

		#endregion

		#region SimpleEntityEventListeners

		public class SimpleEntityCustomValidator : IEventListener<SimpleEntity, ValidateEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name == "EventHandlingExceptionTester")
					throw new EventHandlingException<SimpleEntity, ValidateEvent>("SimpleEntity name is wrong", eventArgs);
			}
		}

		#endregion

		#region CustomizedEventListeners

		public class CustomEntityEventArgs : EntityEventArgs<SimpleEntity>
		{
			public string Message { get; set; }
		}

		public class CustomEventListener : IEventListener<SimpleEntity, CustomEventType>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				var args = eventArgs as CustomEntityEventArgs;
				Assert.NotNull(args);
				Assert.NotNull(args.Entity);
				Assert.Equal("CustomizedSimpleEntity", args.Entity.Name);
				Assert.Equal("Hello", args.Message);
				_message3 = "Custom event was handled";
			}
		}

		public class CustomEventType : IEventType
		{
		}

		public static class CustomService
		{
			public static void Execute(SimpleEntity category)
			{
				EventDispatcher.RaiseEvent(
					new CustomEntityEventArgs {Entity = category, EntityId = category.Id, Message = "Hello"},
					typeof (CustomEventType));
			}
		}

		#endregion

		#region MultipleEventListeners

		public class SimpleEntityMultiEventListener : IEventListener<SimpleEventEntity, InsertEvent>,
		                                              IEventListener<SimpleEventEntity, InsertedEvent>
		{
			public static int Counter;

			void IEventListener<SimpleEventEntity, InsertedEvent>.HandleEvent(EntityEventArgs<SimpleEventEntity> item)
			{
				Counter += 10;
				if (item.Entity.Name.StartsWith("ChangeMe"))
					item.Entity.Name += " created";
			}

			void IEventListener<SimpleEventEntity, InsertEvent>.HandleEvent(EntityEventArgs<SimpleEventEntity> item)
			{
				Counter += 100;
				if (item.Entity.Name.StartsWith("ChangeMe"))
					item.Entity.Name += " create";
			}
		}

		#endregion

		#region FullCycleSimpleEntityEventListeners

		public class SimpleEntityInsertNotifier : IEventListener<SimpleEntity, InsertEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Insert";
			}
		}

		//public class SimpleEntityInsertingNotifier : IEventListener<SimpleEntity, InsertingEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Inserting";
		//    }
		//}

		//public class SimpleEntityInsertedPreCommitNotifier : IEventListener<SimpleEntity, InsertedPreCommitEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Inserted pre commit";
		//    }
		//}

		public class SimpleEntityInsertedNotifier : IEventListener<SimpleEntity, InsertedEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Inserted";
			}
		}

		//public class SimpleEntityLoadingNotifier : IEventListener<SimpleEntity, LoadingEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (!string.IsNullOrEmpty(eventArgs.Entity.Name)) return;
		//        _message1 += "->Loading";
		//    }
		//}

		public class SimpleEntityLoadedNotifier : IEventListener<SimpleEntity, LoadedEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Loaded";
			}
		}

		public class SimpleEntityUpdateNotifier : IEventListener<SimpleEntity, UpdateEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Update";
			}
		}

		//public class SimpleEntityUpdatingNotifier : IEventListener<SimpleEntity, UpdatingEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Updating";
		//    }
		//}

		//public class SimpleEntityUpdatedPreCommitNotifier : IEventListener<SimpleEntity, UpdatedPreCommitEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Updated pre commit";
		//    }
		//}

		public class SimpleEntityUpdatedNotifier : IEventListener<SimpleEntity, UpdatedEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Updated";
			}
		}

		public class SimpleEntityDeleteNotifier : IEventListener<SimpleEntity, DeleteEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Delete";
			}
		}

		//public class SimpleEntityDeletingNotifier : IEventListener<SimpleEntity, DeletingEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Deleting";
		//    }
		//}

		//public class SimpleEntityDeletedPreCommitNotifier : IEventListener<SimpleEntity, DeletedPreCommitEvent>
		//{
		//    public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
		//    {
		//        if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
		//        _message1 += "->Deleted pre commit";
		//    }
		//}

		public class SimpleEntityDeletedNotifier : IEventListener<SimpleEntity, DeletedEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEntity> eventArgs)
			{
				if (eventArgs.Entity.Name != "FullCycleEventsHandlingTester") return;
				_message1 += "->Deleted";
			}
		}

		#endregion

		#region LongRunningSyncAsyncEventListeners

		public class LongRunningAsyncEvenListener : IAsyncEventListener<SimpleEventEntity, InsertEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEventEntity> eventArgs)
			{
				if (!eventArgs.Entity.Name.StartsWith("AsyncCreated should be aborted"))
				{
					return;
				}

				Thread.Sleep(1000); // simulate call to external system 

				throw new TransactionManagerCommunicationException("Could not connect to remote system, rollback.");
			}
		}

		public class LongRunningEvenListener : IEventListener<SimpleEventEntity, InsertEvent>
		{
			public void HandleEvent(EntityEventArgs<SimpleEventEntity> eventArgs)
			{
				if (!eventArgs.Entity.Name.StartsWith("Created should be aborted"))
				{
					return;
				}

				Thread.Sleep(1000); // simulate call to external system 

				throw new TransactionManagerCommunicationException("Could not connect to remote system, rollback.");
			}
		}

		#endregion
	}
}