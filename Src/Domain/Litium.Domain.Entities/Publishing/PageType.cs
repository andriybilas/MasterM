using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Page types; are used to define a set of specified templates.
	/// </summary>
	public class PageType : Entity
	{
		/// <summary>
		/// 	Gets or sets the behavior.
		/// </summary>
		/// <value>The behavior.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual PageBehaviour Behaviour { get; set; }

		/// <summary>
		/// 	The name of the page type.
		/// </summary>
		/// <value>The name.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the page type category.
		/// </summary>
		/// <value>The page type category.</value>
        [Required(ErrorMessageResourceName = "EnumFormat", ErrorMessageResourceType = typeof(Notification))]
		public virtual PageTypeCategories PageTypeCategory { get; set; }

        /// <summary>
        /// Create a shallow copy of object.
        /// </summary>
        /// <returns>new PageType object.</returns>
        public override object Clone()
        {
        	var clone = (PageType)base.Clone();
			if (Behaviour != null)
				clone.Behaviour = (PageBehaviour)Behaviour.Clone();
        	return clone;
        }
	}
}