using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;

namespace Web.Site.Models
{
	public class CategoryModel
	{
		public virtual Guid? Id { get; set; }

		public virtual String Name { get; set; }

		public virtual String Description { get; set; }

		public virtual CategoryModel Parent { get; set; }

		public virtual bool HasChild { get; set; }

		public bool HasImage { get; set; }
	}

	public static class CategoryModelExtension
	{
		public static CategoryModel Copy (this CategoryModel categoryModel, Category category )
		{
			categoryModel.Id = category.Id;
			categoryModel.Name = category.Name;
			categoryModel.Description = category.Description;
		    categoryModel.HasImage = category.HasImage;
			
			if(category.Parent != null)
			{
				categoryModel.Parent = new CategoryModel();
				categoryModel.Parent.Copy(category.Parent);
			}
			else
			{
				categoryModel.Parent = null;	
			}

			return categoryModel;
		}
	}
}