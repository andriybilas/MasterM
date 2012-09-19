using Litium.Common;
using Litium.Domain.Entities.Media;
using Litium.Test.Common.Xunit.Base;
using Xunit;

namespace Litium.Test.Domain.Xunit.Mappings
{
	public class DynamicMetadataTest : TransactionalTestBase
	{
		[Fact]
		public void CanInsertGetUpdateMetadata()
		{
			//var folder = new Folder {Name = "Test folder"};
			//Repository.Data.Save(folder);
			
			//var file = new File
			//{
			//    ContentType = "File",
			//    DisplayName = "File1",
			//    Folder = folder
			//};

			//file.FieldBag.SimpleInt = 10;
			//file.FieldBag.SimpleString = "Simple String";
			//file.FieldBag.SimpleBoolean = true;
			//Repository.Data.Save(file);

			//Repository.Data.Cache.Clear(file);

			//var file2 = Repository.Data.Get<File>(file.Id);
			//Assert.Equal(10, file2.FieldBag.SimpleInt);
			//Assert.Equal("Simple String", file2.FieldBag.SimpleString);
			//Assert.Equal(true, file2.FieldBag.SimpleBoolean);

			//file2.FieldBag.SimpleString = "New String";
			//Repository.Data.Save(file2);

			//Repository.Data.Cache.Clear(file2);

			//var file3 = Repository.Data.Get<File>(file2.Id);
			//Assert.Equal(10, file3.FieldBag.SimpleInt);
			//Assert.Equal("New String", file3.FieldBag.SimpleString);
			//Assert.Equal(true, file3.FieldBag.SimpleBoolean);
		}
	}
}
