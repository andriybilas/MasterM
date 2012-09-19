using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Litium.Common;
using Litium.Common.Configurations;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Media;
using Litium.Test.Common.Xunit.Base;
using Xunit;
using File = Litium.Domain.Entities.Media.File;

namespace Litium.Test.Domain.Xunit.EventListeners
{
	public class FileStorageHandlerTest : ConversationalTestBase
	{
		public static string ConfiguredStorageDirectory
		{
			get
			{
				string path = LitiumConfigs.Data.FilesStorage;
				if (string.IsNullOrEmpty(path))
				{
					throw new ConfigurationErrorsException(string.Format("Files storage is not configured."));
				}
				return path;
			}
		}

		[Fact]
		public void FileContentIsStoredLoadedDeletedTogetherWithFileEntity()
		{
			//Create new file with content, save it and check if file was stored on the disk.
			var folder = new Folder
			             	{
			             		Name = "Test folder",
			             	};
			Repository.Data.Save(folder);

			var file = new File
			           	{
			           		//Folder = folder,
			           		DisplayName = "Test file",
			           		Name = "TestFile.txt",
							ContentType = FileType.txt
			           	};
			Stream stream = new MemoryStream();
			byte[] buffer = Guid.NewGuid().ToByteArray();
			stream.Write(buffer, 0, buffer.Length);
			file.FileStream = new Lazy<Stream>(() => stream);
			Repository.Data.Save(file);
			Assert.NotEmpty(file.StoragePath);
			Assert.Equal(file.Size, buffer.Length);
			string path = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, file.StoragePath);
			var fileInfo = new FileInfo(path);
			Assert.True(fileInfo.Exists);
			Assert.Equal(fileInfo.Length, buffer.Length);

			//Get file and check if file content is initialized.
			Repository.Data.Cache.Clear(file);
			var retrievedFile = Repository.Data.Get<File>(file.Id);
			Assert.False(string.IsNullOrEmpty(retrievedFile.StoragePath));
			Assert.Equal(retrievedFile.Size, buffer.Length);
			Assert.NotEqual(retrievedFile.FileStream.Value, null);
			Assert.Equal(retrievedFile.FileStream.Value.Length, buffer.Length);

			//Initialize file with the new file content, save it and check if old file content was deleted from the disk and new one was stored.
			byte[] newBuffer = Guid.NewGuid().ToByteArray().Take(5).ToArray();
			Stream newStream = new MemoryStream();
			newStream.Write(newBuffer, 0, newBuffer.Length);
			retrievedFile.FileStream = new Lazy<Stream>(() => newStream);
			retrievedFile.Name = "NewTestFile.txt";
			Repository.Data.Save(retrievedFile);
			Assert.NotEmpty(retrievedFile.StoragePath);
			Assert.Equal(retrievedFile.Size, newBuffer.Length);
			string newPath = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, retrievedFile.StoragePath);
			var newFileInfo = new FileInfo(newPath);
			Assert.True(newFileInfo.Exists);
			Assert.Equal(newFileInfo.Length, newBuffer.Length);
			Assert.False(System.IO.File.Exists(path));

			//Delete file and check if file content was also deleted from the disk.
			Repository.Data.Delete(retrievedFile);
			Repository.Data.Cache.Clear(retrievedFile);
			var deletedFile = Repository.Data.Get<File>(retrievedFile.Id);
			Assert.Null(deletedFile);
			Assert.False(System.IO.File.Exists(newPath));
		}

		[Fact]
		public void FileContentIsntStoredIfTransactionWasRolledBack()
		{
			string storagePath = string.Empty;

			using (var unitOfWork = new UnitOfWork())
			{
				var folder = new Folder
				{
					Name = "Test folder",
				};
				Repository.Data.Save(folder);

				var file = new File
				{
					//Folder = folder,
					DisplayName = "Test file",
					Name = "TestFile.txt",
					ContentType = FileType.txt
				};
				Stream stream = new MemoryStream();
				byte[] buffer = Guid.NewGuid().ToByteArray();
				stream.Write(buffer, 0, buffer.Length);
				file.FileStream = new Lazy<Stream>(() => stream);
				Repository.Data.Save(file);
				storagePath = file.StoragePath;

				unitOfWork.Rollback();
			}

			string path = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, storagePath);
			Assert.False(System.IO.File.Exists(path));
		}

		[Fact]
		public void FileContentIsntDeletedIfTransactionWasRolledBack()
		{
			var folder = new Folder
			{
				Name = "Test folder"
			};
			Repository.Data.Save(folder);

			var file = new File
			{
				//Folder = folder,
				DisplayName = "Test file",
				Name = "TestFile.txt",
				ContentType = FileType.txt
			};
			Stream stream = new MemoryStream();
			byte[] buffer = Guid.NewGuid().ToByteArray();
			stream.Write(buffer, 0, buffer.Length);
			file.FileStream = new Lazy<Stream>(() => stream);
			Repository.Data.Save(file);

			using (var unitOfWork = new UnitOfWork())
			{
				Repository.Data.Delete(file);

				unitOfWork.Rollback();
			}

			string path = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, file.StoragePath);
			Assert.True(System.IO.File.Exists(path));

			Repository.Data.Delete(file);
		}
	}
}