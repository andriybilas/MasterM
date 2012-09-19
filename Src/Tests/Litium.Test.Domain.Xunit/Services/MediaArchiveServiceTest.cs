using System;
using System.Collections.Generic;
using Litium.Common;
using Litium.Domain.Entities.Media;
using Litium.Domain.Services;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Domain.Xunit.Mappings;
using Xunit;

namespace Litium.Test.Domain.Xunit.Services
{
    public class MediaArchiveServiceTest : TransactionalTestBase
    {
        private readonly IMediaArchiveService _service;

        public MediaArchiveServiceTest ()
        {
            _service = ServiceProvider.GetService<IMediaArchiveService>();
        }

        [Fact]
        public void MoveFolderTestEnterParameterFolderTest()
        {
			var folder1 = new Folder { Name = "Folder1" };
			Repository.Data.Save(folder1);
			
			var folder2 = new Folder { Name = "Folder2" };
            Repository.Data.Save(folder2);

			_service.Move(folder1, folder2);
			Assert.Equal(folder1, folder2.Parent);
        }

        [Fact]
        public void MoveFolderEnterParameterFolderId()
        {
			var folder1 = new Folder { Name = "Folder1" };
			Repository.Data.Save(folder1);
			
			var folder2 = new Folder { Name = "Folder2" };
            Repository.Data.Save(folder2);
            
			_service.Move(folder1.Id, folder2);

			Assert.Equal(folder1, folder2.Parent);
        }

        [Fact]
        public void MoveFolderCheckEnterParametersTest()
        {
        	Assert.Throws<NullReferenceException>(() => _service.Move((Folder)null, (Folder)null));

            Folder folder1 = new Folder{Name = "Folder1"};
            Folder folder2 = new Folder();

            Assert.Throws<ArgumentOutOfRangeException>(() => _service.Move(folder1.Id, folder2));

            Repository.Data.Save(folder1);

            Assert.Throws<ArgumentOutOfRangeException>(() => _service.Move(folder1.Id, folder2));
        }

		[Fact]
        public void CopyFileEnterParameterFolderIdTest()
        {
			var folder1 = new Folder { Name = "Folder1" };
            Repository.Data.Save(folder1);
			File file1 = new File { ContentType = FileType.txt, Name = "Dummy", DisplayName = "Dummy"};
			Repository.Data.Save(file1);
			File copyFile1 = _service.Copy(folder1.Id, file1);

			//Assert.Equal(folder1, copyFile1.Folder);
        }


    }
}
