using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	The template define the physical file that is used to render the site.
	/// </summary>
	public class PageDisplayTemplate : Entity
	{
		/// <summary>
		/// 	Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string FileName { get; set; }

		/// <summary>
		/// 	Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the type of the page.
		/// </summary>
		/// <value>The type of the page.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual PageType PageType { get; set; }

		/// <summary>
		/// 	Gets or sets the thumbnail path.
		/// </summary>
		/// <value>The thumbnail path.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string ThumbnailPath { get; set; }
	}
}