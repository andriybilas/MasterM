using System.Collections.Generic;
using Litium.Common;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.GlobalRepository
{
	public class RepositoryTest : ConversationalTestBase
	{
		[Fact]
		public void AllInOneTest()
		{
			var entity1 = new SimpleEntity { Name = "Repository Test 1" };
			var entity2 = new SimpleEntity { Name = "Repository Test 2" };
			var entity3 = new SimpleEntity { Name = "Repository Test 3" };
			var entity4 = new SimpleEntity { Name = "Repository Test 4" };
			var entity5 = new SimpleEntity { Name = "Repository Test 5" };
			var entity6 = new SimpleEntity { Name = "Repository Test Query 6" };
			var entity7 = new SimpleEntity { Name = "Repository Test Query 7" };

			Repository.Data.Save(entity1);
			Repository.Data.Save(entity2, entity3, entity4);
			Repository.Data.Save(new List<SimpleEntity>{entity5, entity6, entity7});

			var all = new List<SimpleEntity>{entity1, entity2, entity3, entity4, entity5, entity6, entity7};
			foreach (var entity in all)
			{
				var data = Repository.Data.Get<SimpleEntity>(entity.Id);
				Assert.NotNull(data);
				Assert.Equal(entity.Name, data.Name);
			}

			Repository.Data.Delete(entity1);
			Repository.Data.Delete(entity2, entity3);
			Repository.Data.Delete(new List<SimpleEntity> { entity4, entity5 });
			Repository.Data.Delete<SimpleEntity>(x => x.Name.StartsWith("Repository Test Query"));

			foreach (var entity in all)
			{
				var data = Repository.Data.Get<SimpleEntity>(entity.Id);
				Assert.Null(data);
			}
		}
	}
}
