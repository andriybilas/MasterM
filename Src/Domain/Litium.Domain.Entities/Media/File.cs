using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Media
{
	/// <summary>
	/// 	Base file entity for files, images and versions
	/// </summary>
	[MetadataType (typeof (FileMetadata))]
	public class File : Entity
	{
	    /// <summary>
		/// 	Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public virtual FileType ContentType { get; set; }

        public virtual Guid EntityId { get; set; }

		/// <summary>
		/// 	Gets or sets the display name. Required.
		/// </summary>
		/// <value>The display name.</value>
		public virtual string DisplayName { get; set; }

		/// <summary>
		/// 	Gets or sets the length of the file.
		/// </summary>
		/// <value>The length of the file.</value>
		public virtual int? Size { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the storage ID.
		/// </summary>
		/// <value>The path with in the storage.</value>
		public virtual string StoragePath { get; set; }

        /// <summary>
        /// 	Gets or sets the storage ID.
        /// </summary>
        /// <value>The path with in the storage.</value>
        public virtual ResizedVersion ResizedTo { get; set; }

		/// <summary>
		/// 	Gets or sets the file content as stream.
		/// </summary>
		/// <value>The file content stream.</value>
		public virtual Lazy<Stream> FileStream { get; set; }

		public override object Clone()
		{
			return new File {
			       		ContentType = ContentType,
			       		DisplayName = DisplayName,
			       		Name = Name,
			       		Size = Size,
                        ResizedTo = ResizedTo,
			       		FileStream = new Lazy<Stream>(() => FileStream != null ? FileStream.Value : null),
			       		StoragePath = StoragePath };
		}

		public override object ValidationCopy()
		{
			return Clone();
		}
	}

    public class FileMetadata
	{
		[Required (ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (DomainNotification))]
		public virtual ContentType ContentType { get; set; }

		[Required (ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (DomainNotification))]
		public virtual string DisplayName { get; set; }
	}
}