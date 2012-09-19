using System;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using Litium.Common;
using Litium.Common.Validation;
using Litium.Domain.Entities.Media;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;
using File = Litium.Domain.Entities.Media.File;

namespace Litium.Test.Domain.Xunit.Validation
{
	public class MediaArchiveValidationTest : ConversationalTestBase
	{
		[Fact]
		public void ValidationFileInsertTest()
		{
			var folder = new Folder {Name = "Folder1"};
			Repository.Data.Save(folder);
			Assert.NotNull(folder);

			var file = new File
			           	{
							ContentType = FileType.image,
			           		DisplayName = string.Empty,
			           		Name = string.Empty,
			           	};

			Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(file));
		}

		[Fact]
		public void ValidationFolderInsertTest()
		{
			var folder = new Folder
			             	{
			             		Name = string.Empty //Value is required
			             	};

			Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(folder));
		}

		[Fact]
		public void ValidationLocalizationTest()
		{
			var test = new LocalizationTestEntity();
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			try
			{
				Repository.Data.Save(test);
			}
			catch (ValidationArgumentException ex)
			{
				Assert.True(ex.ValidationResult.Select(x => x.Message).First().Contains("English"));
			}

			Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv");

			try
			{
				Repository.Data.Save(test);
			}
			catch (ValidationArgumentException ex)
			{
				Assert.True(ex.ValidationResult.Select(x => x.Message).First().Contains("Swedish"));
			}
		}

        [Fact]
        public void TryCreateIdenticalFolder ()
        {
            string folderName = "Folder - " + Guid.NewGuid().ToString();
            var folder1 = new Folder { Name = folderName, Parent = null };
            Repository.Data.Save(folder1);
            var folder2 = folder1.Clone();
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(folder2));
        }
	}
}