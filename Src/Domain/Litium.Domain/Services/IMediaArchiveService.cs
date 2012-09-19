using System;
using Litium.Domain.Entities.Media;

namespace Litium.Domain.Services
{
    /// <summary>
    /// The MediaArchive Service interface.
    /// </summary>
    public interface IMediaArchiveService
    {
        /// <summary>
        /// Move folder from one parent folder to another parent folder.
        /// </summary>
        /// <param name="newParentFolderId">Parent folder Id.</param>
        /// <param name="folder">Folder which is moved.</param>
        void Move(Guid newParentFolderId, Folder folder);

        /// <summary>
        /// Override. Move folder from one parent folder to another parent folder.
        /// </summary>
        /// <param name="newParentFolder">Parent folder.</param>
        /// <param name="folder">Folder which is moved.</param>
        void Move(Folder newParentFolder, Folder folder);

        /// <summary>
        /// Creates a copy of the file in a new parent folder.
        /// </summary>
        /// <param name="newParentFolderId">Parent folder Id.</param>
        /// <param name="file">File what will be copied into new folder.</param>
        /// <returns>The file copy, without file value.</returns>
        File Copy(Guid newParentFolderId, File file);

        /// <summary>
        /// Override. Creates a copy of the file in a new parent folder.
        /// </summary>
        /// <param name="newParentFolder">New parent folder.</param>
        /// <param name="file">Copied file.</param>
        /// <returns>The file copy, without file value.</returns>
        File Copy(Folder newParentFolder, File file);

        /// <summary>
        /// Move file from a folder to another folder.
        /// </summary>
        /// <param name="newParentFolder">New folder.</param>
        /// <param name="file">The file which will be moved.</param>
        void Move(Folder newParentFolder, File file);

        /// <summary>
        ///  Override. Move file from a folder to another folder.
        /// </summary>
        /// <param name="newParentFolderId">New folder Id.</param>
        /// <param name="file">The file.</param>
        void Move(Guid newParentFolderId, File file);
    }
}