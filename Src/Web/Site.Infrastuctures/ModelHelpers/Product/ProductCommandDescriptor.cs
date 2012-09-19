using System;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class ProductCommandDescriptor
    {
        public Guid EnityId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

	public class BrandCommandDescriptor : CategoryCommandDescriptor
	{
		public String Brand { get; set; }
	}

    public class CategoryCommandDescriptor : ProductCommandDescriptor
    {

    }

    public class ProductSetCommandDescriptor : ProductCommandDescriptor
    {

    }

    public class CampaignCommandDescriptor : ProductCommandDescriptor
    {

    }

	public class SearchCommandDescriptor : ProductCommandDescriptor
	{
		public String KeyWord { get; set; }
	}
}