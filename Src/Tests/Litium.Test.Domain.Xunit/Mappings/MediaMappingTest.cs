using System;
using FluentNHibernate.Testing;
using Litium.Domain.Entities.Media;
using Litium.Test.Domain.Xunit.Mappings.Base;
using Xunit;

namespace Litium.Test.Domain.Xunit.Mappings
{
	public class MediaMappingTest : MappingTestBase
	{
		[Fact]
		public File MapFile()
		{
			return new PersistenceSpecification<File>(Session)
				.CheckProperty(x => x.ContentType, FileType.jpeg)
				.CheckProperty(x => x.DisplayName, "Display name")
				.CheckProperty(x => x.Size, 1024)
				.CheckProperty(x => x.Name, "File name")
				.CheckProperty(x => x.StoragePath, "c:\\temp\\test_file.png")
				.VerifyTheMappings();
		}

		[Fact]
		public Folder MapFolder()
		{
			var uniqueFolderName = "Folder_" + Guid.NewGuid();

			return new PersistenceSpecification<Folder>(Session)
				.CheckProperty(x => x.Name, uniqueFolderName)
				.CheckProperty(x => x.Parent, null)
				.VerifyTheMappings();
		}
	}
}