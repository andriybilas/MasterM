using System;
using System.Collections.Generic;
using Litium.Test.Common.Xunit.Base;
using Xunit;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	public class StateEngineTests : TransactionalTestBase
	{
		public enum TestStatus
		{
			Status1,
			Status2,
			Status3,
			Status4,
			Status5,
			Status6
		}

		[Fact]
		public void StateEngineTest()
		{
			var stateDefinition = new StateDefinition<TestEntity, TestStatus>(
				x => x.Status,
				new StateTransitionDefinition<TestEntity, TestStatus>(TestStatus.Status1, TestStatus.Status2),
				new StateTransitionDefinition<TestEntity, TestStatus>(TestStatus.Status2, TestStatus.Status3)
					{
						ExpectParameterType = typeof (StateChangeArgument),
						Expect = (entity, o) => entity.Allow,
						PreAction = (container, entity, arg) =>
						            	{
						            		/* do something before action */
						            	},
						PostAction = (container, entity, arg) =>
						             	{
						             		entity.Date = arg.Date;
						             		//container.OnCommitAction(() =>
						             		//                            {
						             		//                                // Code to send
						             		//                            });
						             		container.StateEngine.Execute(entity, TestStatus.Status4);
						             	}
					},
				new StateTransitionDefinition<TestEntity, TestStatus>(TestStatus.Status3, TestStatus.Status4),
				new StateTransitionDefinition<TestEntity, TestStatus>(TestStatus.Status4, TestStatus.Status5),
				new StateTransitionDefinition<TestEntity, TestStatus>(TestStatus.Status4, TestStatus.Status6));

			var testEntity = new TestEntity();
			var stateChangeArgument = new StateChangeArgument {Date = DateTime.Now.AddDays(1)};

			using (var uow = new DummyUnitOfWorkContainer())
			{
				Assert.Equal(TestStatus.Status1, testEntity.Status);

				// state is not added and should throw state not found
				Assert.Throws<StateNotFoundForEntityException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status2));
			}

			// Add state definition to the static collection
			StateEngine.Add(stateDefinition);

			using (var uow = new DummyUnitOfWorkContainer())
			{
				Assert.Equal(TestStatus.Status1, testEntity.Status);

				var allowedStates = uow.StateEngine.GetAvailableTransitions<TestEntity, TestStatus>(testEntity);
				Assert.Equal(new HashSet<TestStatus>(new[] {TestStatus.Status2}), allowedStates);

				Assert.Throws<StateTransitionArgumentException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status2, stateChangeArgument));
				Assert.Equal(TestStatus.Status1, testEntity.Status);

				uow.StateEngine.Execute(testEntity, TestStatus.Status2);
				Assert.Equal(TestStatus.Status2, testEntity.Status);

				allowedStates = uow.StateEngine.GetAvailableTransitions<TestEntity, TestStatus>(testEntity);
				Assert.Equal(new HashSet<TestStatus>(new[] {TestStatus.Status3}), allowedStates);

				Assert.Throws<StateTransitionArgumentException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status3));
				Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status3, stateChangeArgument));
				Assert.Equal(TestStatus.Status2, testEntity.Status);

				testEntity.Allow = true;
				Assert.Throws<StateTransitionArgumentException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status3));
				Assert.Equal(TestStatus.Status2, testEntity.Status);

				uow.StateEngine.Execute(testEntity, TestStatus.Status3, stateChangeArgument);
				Assert.Equal(stateChangeArgument.Date, testEntity.Date);
				// Post action move status one more step so the status should be Status4
				Assert.Equal(TestStatus.Status4, testEntity.Status);

				allowedStates = uow.StateEngine.GetAvailableTransitions<TestEntity, TestStatus>(testEntity);
				Assert.Equal(new HashSet<TestStatus>(new[] {TestStatus.Status5, TestStatus.Status6}), allowedStates);

				Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status1));
				Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(testEntity, TestStatus.Status2));
			}
		}

		public class StateChangeArgument
		{
			public DateTime Date { get; set; }
		}

		public class TestEntity
		{
			public virtual bool Allow { get; set; }
			public virtual DateTime Date { get; protected internal set; }
			public virtual TestStatus Status { get; protected set; }
		}
	}
}
