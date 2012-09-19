using Litium.Common;
using Litium.Common.Lifecycle;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.WorkUnit
{
	public class AutoMergeTest : TransactionalTestBase
	{
		[Fact]
		public void TryToAutoMergeIfNeeded()
		{
			var item = new SimpleEntity
			           	{
			           		Name = "PageType #1"
			           	};
			Repository.Data.Save(item);
			ConversationHelper.ReOpen();

			item = Repository.Data.Get<SimpleEntity>(item.Id);
			item.Name += ".2";

			Repository.Data.Save(item);

			ConversationHelper.ReOpen();
			Repository.Data.Get<SimpleEntity>(item.Id);

			item.Name += ".3";
			Repository.Data.Save(item);
		}
	}
}