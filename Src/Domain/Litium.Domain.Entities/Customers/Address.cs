using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Customers
{
	/// <summary>
	/// 	Address information
	/// </summary>
    [MetadataType(typeof(AddressMetadata))]
    public class Address : ComparableObject, ICloneable
	{
		/// <summary>
		/// 	Gets or sets the address2.
		/// </summary>
		/// <value>The address2.</value>
		public virtual string Address1 { get; set; }

		/// <summary>
		/// 	Gets or sets the address2.
		/// </summary>
		/// <value>The address2.</value>
		[NotSerializable]
		public virtual string Address2 { get; set; }

		/// <summary>
		/// 	Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		public virtual string City { get; set; }

        /// <summary>
        /// Create a shallow copy of object.
        /// </summary>
        /// <returns>new Address object.</returns>
        public object Clone()
        {
        	return MemberwiseClone();
        }
	}

    public class AddressMetadata
    {
        [ResourceDisplayName(ResourceKey.Address1)]
        public virtual string Address1 { get; set; }

        [ResourceDisplayName(ResourceKey.Address2)]
        public virtual string Address2 { get; set; }

        [ResourceDisplayName(ResourceKey.City)]
        public virtual string City { get; set; }
    }
}