using System;
using System.Configuration;
using System.IO;
using Litium.Common.Configurations;
using File = Litium.Domain.Entities.Media.File;

namespace Litium.Domain.Utilities
{
	internal static class FileStorageUtilities
	{
		private const string _storagePrefix = "Media";

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

		/// <summary>
		/// Saves the file to the storage.
		/// </summary>
		/// <param name="file">File to store</param>
		/// <returns></returns>
		public static void StoreFile(File file)
		{
			Guid id = Guid.NewGuid();

			//Check if directory exists, if not create.
			//string relativeDirPath = GetStorageDirectoryPath(id, file.Name);
			//string fullDirPath = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, relativeDirPath);
		    string fullDirPath = ConfiguredStorageDirectory;

            if (!Directory.Exists(file.StoragePath))
            {
                Directory.CreateDirectory(file.StoragePath);
            }

			//Store the file
			//string relativeFilePath = GetStorageFilePath(id, file.Name, relativeDirPath);
			//string fullFilePath = string.Format(@"{0}\{1}", ConfiguredStorageDirectory, relativeFilePath);

            FileInfo fileInfo = new FileInfo(string.Format(@"{0}\{1}", file.StoragePath, file.Name));

			var fileSize = 0;
			using (Stream fileStream = fileInfo.OpenWrite())
			{
				const int chunkSize = 1000;
				byte[] buffer = new byte[chunkSize];

				//Read from stream.
				file.FileStream.Value.Seek(0, SeekOrigin.Begin);
				int lengthRead = file.FileStream.Value.Read(buffer, 0, chunkSize);
				while (lengthRead > 0)
				{
					//Write 
					fileStream.Write(buffer, 0, lengthRead);
					fileStream.Flush(); //To ensure small memory buffers.
					fileSize += lengthRead;

					//Read next section.
					lengthRead = file.FileStream.Value.Read(buffer, 0, chunkSize);
				}
			}

			file.Size = fileSize;
			//file.StoragePath = relativeFilePath;
		}

		/// <summary>
		/// Deletes the file from the storage.
		/// </summary>
		/// <param name="file">File to delete</param>
		/// <returns></returns>
		public static void DeleteFile(File file)
		{
			string filePath = string.Format(@"{0}\{1}", file.StoragePath, file.Name);
			
            FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
				return;

            fileInfo.Delete();
		}

		/// <summary>
		/// Loads the file from the storage.
		/// </summary>
		/// <param name="file">File to load</param>
		/// <returns></returns>
		public static Stream LoadFile(File file)
		{
			string filePath = string.Format(@"{0}\{1}", file.StoragePath, file.Name);
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
				return null;

			var memoryStream = new MemoryStream();
			using (var stream = fileInfo.OpenRead())
			{
				stream.CopyTo(memoryStream);
			}
			memoryStream.Seek(0, SeekOrigin.Begin);

			return memoryStream;
		}
	}
}