using Xunit;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	public class StateEngineTests2
	{
		public enum DeliveryState
		{
			Init,
			Prepare,
			Sent,
			Done
		}

		public enum OrderState
		{
			Init,
			Confirmed,
			Processing,
			Done
		}

		[Fact]
		public void StateEngineTest()
		{
			{
				var orderState = new StateDefinition<Order, OrderState>(
					x => x.State,
					new StateTransitionDefinition<Order, OrderState>(OrderState.Init, OrderState.Confirmed),
					new StateTransitionDefinition<Order, OrderState>(OrderState.Confirmed, OrderState.Processing),
					new StateTransitionDefinition<Order, OrderState>(OrderState.Processing, OrderState.Done)
						{
							Expect = (entity, o) => entity.Delivery.State == DeliveryState.Done
						}
					);

				var deliveryState = new StateDefinition<Delivery, DeliveryState>(
					x => x.State,
					new StateTransitionDefinition<Delivery, DeliveryState>(DeliveryState.Init, DeliveryState.Prepare)
						{
							Expect = (entity, o) => entity.Order.State == OrderState.Processing
						},
					new StateTransitionDefinition<Delivery, DeliveryState>(DeliveryState.Prepare, DeliveryState.Sent)
						{
							Expect = (entity, o) => entity.Order.State == OrderState.Processing
						},
					new StateTransitionDefinition<Delivery, DeliveryState>(DeliveryState.Sent, DeliveryState.Done)
						{
							PostAction = (container, entity, arg3) => container.StateEngine.Execute(entity.Order, OrderState.Done)
						}
					);

				StateEngine.Add(orderState);
				StateEngine.Add(deliveryState);
			}

			{
				var order = new Order();
				var delivery = new Delivery {Order = order};
				order.Delivery = delivery;

				using (var uow = new DummyUnitOfWorkContainer())
				{
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(delivery, DeliveryState.Prepare));
					uow.StateEngine.Execute(order, OrderState.Confirmed);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(delivery, DeliveryState.Prepare));
					uow.StateEngine.Execute(order, OrderState.Processing);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(order, OrderState.Done));
					uow.StateEngine.Execute(delivery, DeliveryState.Prepare);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(order, OrderState.Done));
					uow.StateEngine.Execute(delivery, DeliveryState.Sent);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(order, OrderState.Done));
					uow.StateEngine.Execute(delivery, DeliveryState.Done);
					Assert.Equal(OrderState.Done, order.State);
				}
			}
		}

		public class Delivery
		{
			public Order Order { get; set; }
			public virtual DeliveryState State { get; set; }
		}

		public class Order
		{
			public virtual Delivery Delivery { get; set; }
			public virtual OrderState State { get; protected set; }
		}
	}
}
