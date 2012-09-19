using System;
using Xunit;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	public class StateEngineTests3
	{
		[Fact]
		public void StateEngineTest()
		{
			{
				// Setup page state
				var pageState = new StateDefinition<Page, PageStatus>(
					x => x.Status,
					new StateTransitionDefinition<Page, PageStatus>(PageStatus.Unpublished, PageStatusExtended.ForTranslation),
					new StateTransitionDefinition<Page, PageStatus>(PageStatusExtended.ForTranslation, PageStatusExtended.ReadToPublish),
					new StateTransitionDefinition<Page, PageStatus>(PageStatusExtended.ReadToPublish, PageStatus.Published),
					new StateTransitionDefinition<Page, PageStatus>(PageStatusExtended.ReadToPublish, PageStatus.DeleyPublished)
						{
							ExpectParameterType = typeof (DeleydPublishArgs),
							Expect = (page, o) => o.Date > DateTime.Now,
							PreAction = (container, page, arg3) => page.TimePublishDate = arg3.Date
						}
					);

				StateEngine.Add(pageState);
			}

			{
				// the actual test
				var page = new Page();

				using (var uow = new DummyUnitOfWorkContainer())
				{
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatus.Published));
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatusExtended.ReadToPublish));
					uow.StateEngine.Execute(page, PageStatusExtended.ForTranslation);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatus.Unpublished));
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatus.Published));
					uow.StateEngine.Execute(page, PageStatusExtended.ReadToPublish);
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatus.Unpublished));
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatusExtended.ForTranslation));
					Assert.Throws<StateTransitionNotAllowedException>(() => uow.StateEngine.Execute(page, PageStatusExtended.ReadToPublish));
					uow.StateEngine.Execute(page, PageStatus.Published);
				}
			}

			Assert.Equal(PageStatus.Get("test"), PageStatus.Get("test"));
			Assert.Equal(PageStatus.Get("test"), PageStatus.Get("test"));
			Assert.Equal(PageStatus.Get("test"), PageStatus.Get("test"));
		}

		public class DeleydPublishArgs
		{
			public virtual DateTime Date { get; set; }
		}

		public class Page
		{
			public Page()
			{
				Status = PageStatus.Unpublished;
			}

			public virtual PageStatus Status { get; protected set; }
			public virtual DateTime? TimePublishDate { get; protected internal set; }
		}

		public class PageStatus : Parameter<PageStatus>
		{
			public static PageStatus DeleyPublished = Get("DelayPublished");
			public static PageStatus Published = Get("Published");
			public static PageStatus Unpublished = Get("Unpublished");

			private PageStatus(string name)
				: base(name)
			{
			}

			public static PageStatus Get(string name)
			{
				Parameter<PageStatus> value;
				if (AllParameters.TryGetValue(MakeKey(name), out value))
				{
					return value as PageStatus;
				}
				return new PageStatus(name);
			}
		}

		public class PageStatusExtended
		{
			public static PageStatus ForTranslation = PageStatus.Get("ForTranslation");
			public static PageStatus ReadToPublish = PageStatus.Get("ReadToPublish");
		}
	}
}
