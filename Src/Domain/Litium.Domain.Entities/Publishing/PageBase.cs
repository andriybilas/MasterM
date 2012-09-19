using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Publishing
{
	public abstract class PageBase : Entity
	{
		/// <summary>
		/// 	Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the page template.
		/// </summary>
		/// <value>The page template.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual PageDisplayTemplate PageTemplate { get; set; }

		/// <summary>
		/// 	Gets or sets the seo.
		/// </summary>
		/// <value>The seo.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual SeoProfile Seo { get; set; }

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public override object Clone()
		{
			var clone = (PageBase)base.Clone();
			if (Seo != null)
				clone.Seo = (SeoProfile)Seo.Clone();
			return clone;
		}
	}
}