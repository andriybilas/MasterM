using System;
using Litium.Common;
using Litium.Domain.Entities.Media;

namespace Litium.Domain.Services.Implementation
{

    /// <summary>
    /// Implements MediaArchive Service interface.
    /// </summary>
    internal class MediaArchiveService : IMediaArchiveService
    {
        /// <summary>
        /// Create file copy.
        /// </summary>
        /// <param name="file">File which will be cloned.</param>
        /// <returns>The new file.</returns>
        private File CloneFile(File file)
        {
            return new File
            {
                ContentType = file.ContentType,
                DisplayName = file.DisplayName,
                Size = file.Size,
                Name = file.Name,
                //Folder = file.Folder
            };
        }

        /// <summary>
        /// Move folder from one parent folder to another parent folder.
        /// </summary>
        /// <param name="newParentFolderId">Parent folder Id.</param>
        /// <param name="folder">Folder which is moved.</param>
        public void Move(Guid newParentFolderId, Folder folder)
        {
            if (folder == null)
                throw new NullReferenceException("The moved folder can't be null");

            if(folder.Id == Guid.Empty)
                throw new ArgumentOutOfRangeException("folderId", "Value can't be Guid.Empty");
            
            folder.Parent = (newParentFolderId == Guid.Empty) ? null : Repository.Data.Get<Folder>(newParentFolderId);;
            Repository.Data.Save(folder);
        }

        /// <summary>
        /// Move folder from one parent folder to another parent folder.
        /// </summary>
        /// <param name="newParentFolder">Parent folder.</param>
        /// <param name="folder">Folder which is moved.</param>
        public void Move(Folder newParentFolder, Folder folder)
        {
            Move(newParentFolder == null ? Guid.Empty : newParentFolder.Id, folder);
        }

        /// <summary>
        /// Creates a copy of the file in a new parent folder.
        /// </summary>
        /// <param name="newParentFolderId">Parent folder Id.</param>
        /// <param name="file">File what will be copied into new folder.</param>
        /// <returns>The file copy, without file value.</returns>
        public File Copy(Guid newParentFolderId, File file)
        {
            if (newParentFolderId == Guid.Empty)
                throw new ArgumentOutOfRangeException("newParentFolderId", "Value can't be Guid.Empty");

            if (file == null)
                throw new NullReferenceException("File can't be null");

            var folder = Repository.Data.Get<Folder>(newParentFolderId);
            
            if (folder == null)
                throw new ArgumentNullException("The parent folder doesn't exist.");

            File newFile = (File)file.Clone();
            //newFile.Folder = folder;
            Repository.Data.Save(newFile);
            return newFile;
        }

        /// <summary>
        /// Creates a copy of the file in a new parent folder.
        /// </summary>
        /// <param name="newParentFolder">New parent folder.</param>
        /// <param name="file">Copied file.</param>
        /// <returns>The file copy, without file value.</returns>
        public File Copy(Folder newParentFolder, File file)
        {
            if (newParentFolder == null)
                throw new NullReferenceException("The new ParentFolder can't be null");

            return Copy(newParentFolder.Id, file);
        } 

        /// <summary>
        /// Move file from a folder to another folder.
        /// </summary>
        /// <param name="newParentFolder">New folder.</param>
        /// <param name="file">The file which will be moved.</param>
        public void Move(Folder newParentFolder, File file)
        {
            if (newParentFolder == null)
                throw new NullReferenceException("The new ParentFolder can't be null");

            if (newParentFolder.Id == Guid.Empty)
                throw new ArgumentOutOfRangeException("newParentFolderId", "Value can't be Guid.Empty");

            if (file == null)
                throw new NullReferenceException("File can't be null");

            //Repository.Data.Save(file.Folder = newParentFolder);
        }

        /// <summary>
        ///  Override. Move file from a folder to another folder.
        /// </summary>
        /// <param name="newParentFolderId">New folder Id.</param>
        /// <param name="file">The file.</param>
        public void Move(Guid newParentFolderId, File file)
        {
            if (newParentFolderId == Guid.Empty)
                throw new ArgumentOutOfRangeException("newParentFolderId", "Value can't be Guid.Empty");

            if (file == null)
                throw new NullReferenceException("File can't be null");
            
            var folder = Repository.Data.Get<Folder>(newParentFolderId);
            
            if (folder == null)
                throw new ArgumentNullException("The parent folder doesn't exist.");
            
            //Repository.Data.Save(file.Folder = folder);
        }
    }
}
