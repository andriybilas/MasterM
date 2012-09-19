using System;
using System.Collections.Generic;
using Litium.Common.Entities;

namespace Litium.Domain.Entities.Publishing
{
	public class PageBehaviour : ComparableObject, ICloneable
	{
		/// <summary>
		/// 	Gets or sets a value indicating whether this instance can be in menu.
		/// </summary>
		/// <value> <c>true</c> if this instance can be in menu; otherwise, <c>false</c> . </value>
		public virtual bool CanBeInMenu { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this instance can be in site map.
		/// </summary>
		/// <value> <c>true</c> if this instance can be in site map; otherwise, <c>false</c> . </value>
		public virtual bool CanBeInSiteMap { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is in analytics.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is in analytics; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsInAnalytics { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this instance can be master page.
		/// </summary>
		/// <value> <c>true</c> if this instance can be master page; otherwise, <c>false</c> . </value>
		public virtual bool CanBeMasterPage { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this instance can be moved to trashcan.
		/// </summary>
		/// <value> <c>true</c> if this instance can be moved to trashcan; otherwise, <c>false</c> . </value>
		public virtual bool CanBeDeleted { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is searchable.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is searchable; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsSearchable { get; set; }

		//public virtual bool CanBeVersioned { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [editable in UI].
		/// </summary>
		/// <value><c>true</c> if [editable in UI]; otherwise, <c>false</c>.</value>
		public virtual bool EditableInUI { get; set; }

		//public virtual int PageVersionsToKeep { get; set; }

		/// <summary>
		/// 	Gets or sets the possible child page types.
		/// </summary>
		/// <value> The possible child page types. </value>
		public virtual ISet<PageType> PossibleChildren { get; set; }

		/// <summary>
		/// 	Gets or sets the possible parent page types.
		/// </summary>
		/// <value> The possible parent page types. </value>
		public virtual ISet<PageType> PossibleParentPageTypes { get; set; }

		/// <summary>
		/// 	Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns> A new object that is a copy of this instance. </returns>
		public object Clone()
		{
			return MemberwiseClone();
			// TODO: Clone collections
		}
	}
}
