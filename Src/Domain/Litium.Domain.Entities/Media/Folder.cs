using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Media
{
	/// <summary>
	/// 	Folder will contains files and folders.
	/// </summary>
	public class Folder : Entity
	{
		/// <summary>
		/// 	Gets or sets the name (required).
		/// </summary>
		/// <value>The name.</value>
		[Required (ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (DomainNotification))]
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public virtual Folder Parent { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}