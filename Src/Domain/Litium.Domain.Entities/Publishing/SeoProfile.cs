using System;
using Litium.Common.Entities;

namespace Litium.Domain.Entities.Publishing
{
	public class SeoProfile : ComparableObject, ICloneable
	{
		/// <summary>
		/// 	Gets or sets the search engine change frequency.
		/// </summary>
		/// <value>The search engine change frequency.</value>
		public virtual ChangeFrequency ChangeFrequency { get; set; }

		/// <summary>
		/// 	Gets or sets the search engine description.
		/// </summary>
		/// <value>The search engine description.</value>
		public virtual string Description { get; set; }

		/// <summary>
		/// 	Gets or sets the search engine keywords.
		/// </summary>
		/// <value>The search engine keywords.</value>
		public virtual string Keywords { get; set; }

		/// <summary>
		/// 	Gets or sets the search engine priority. 
		/// 	Value should be between 0.0 and 1.0.
		/// </summary>
		/// <value>The search engine priority.</value>
		public virtual decimal Priority { get; set; }

		/// <summary>
		/// 	Gets or sets the search engine title.
		/// </summary>
		/// <value>The search engine title.</value>
		public virtual string Title { get; set; }

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}