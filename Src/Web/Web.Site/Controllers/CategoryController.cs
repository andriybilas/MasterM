using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Litium.Common;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Campaign;
using Site.Infrastuctures.ModelHelpers.Product;
using Site.Infrastuctures.Security;
using Telerik.Web.Mvc.UI;
using Web.Site.Models;

namespace Web.Site.Controllers
{
    public class CategoryController : Controller
    {
		[Security(Roles = UserRole.Administer)]
        public ActionResult Index()
        {
            ProductModelService.Service.ClearSearchCriteria();

            IEnumerable<Category> categories = Repository.Data.Get<Category>()
				.Where(x => x.Parent == null).All().OrderBy(x => x.Name);

			var model = categories.Select(x => new CategoryModel {
				Name = x.Name,
				Id = x.Id,
				HasChild = ChildExist(x),
			}).OrderBy(x => x.Name);

            return View("Category", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TreeVeiwLoadAsync (TreeViewItem node)
        {
			IEnumerable<Category> categories;

        	if (node.Value != null)
        		categories = Repository.Data.Get<Category>().Where(x => x.Parent.Id == Guid.Parse(node.Value)).All();
        	else
        		categories = Repository.Data.Get<Category>().Where(x => x.Parent == null).All().OrderBy(x => x.Name);

        	var model = categories.Select(x => new TreeViewItemModel
                                                   {
                                                       Text = x.Name,
                                                       Value = x.Id.ToString(),
                                                       LoadOnDemand = ChildExist(x),
                                                   }).OrderBy(x => x.Text);

            return new JsonResult { Data = model };
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PublicTreeVeiwLoadAsync (TreeViewItem node)
        {
			IEnumerable<Category> categories;

        	if (node.Value != null)
        		categories = Repository.Data.Get<Category>().Where(x => x.Parent.Id == Guid.Parse(node.Value)).All();
        	else
        		categories = Repository.Data.Get<Category>().Where(x => x.Parent == null).All().OrderBy(x => x.Name);

        	var model = categories.Select(x => new TreeViewItemModel
                                                   {
                                                       Text = x.Name,
                                                       Value = x.Id.ToString(),
                                                       LoadOnDemand = ChildExist(x),
                                                       NavigateUrl = String.Format("/Public/Category/{0}/{1}", x.Name, x.Id)
                                                   }).OrderBy(x => x.Text);

            return new JsonResult { Data = model };
        }

        [AcceptVerbs(HttpVerbs.Post)]
		[Security(Roles = UserRole.Administer)]
        public void DragNDrop(String dragNodeId, String parentNodeId)
        {
            var dragCategory = Repository.Data.Get<Category>(Guid.Parse(dragNodeId));
            var parentCategory = Repository.Data.Get<Category>(Guid.Parse(parentNodeId));
            dragCategory.Parent = parentCategory;
            Repository.Data.Save(dragCategory);
        }

		[Security(Roles = UserRole.Administer)]
        private bool ChildExist(Category category)
        {
            IEnumerable<Category> categories =
                Repository.Data.Get<Category>().Where(x => x.Parent.Id == category.Id).All();

            return categories.Count() > 0;
        }

		public ActionResult OpenCategoryEditWindow()
        {
            ViewBag.WindowTitle = StoreResourceStrings.CreateNewCategory;
            ViewBag.ActionName = "LoadCategoryEditForm";
            ViewBag.ControllerName = "Category";
            ViewBag.Width = 325;
            return PartialView("Partial/Window");
        }

        [AcceptVerbs(HttpVerbs.Post)]
		[Security(Roles = UserRole.Administer)]
		public ActionResult SwitchView( string option, string categoryId )
        {
			Guid catId = Guid.Empty;
            Category category = null;
            if (Guid.TryParse(categoryId, out catId))
            {
                category = Repository.Data.Get<Category>(catId);
            }

			if (option.Equals ("create-category", StringComparison.InvariantCultureIgnoreCase))
				return PartialView("Partial/NewCategory");
			if (option.Equals ("assign-category", StringComparison.InvariantCultureIgnoreCase))
                return PartialView("Partial/ProductAssign");
			if (option.Equals ("edit-category", StringComparison.InvariantCultureIgnoreCase))
                return PartialView("Partial/NewCategory", category);

			return PartialView ("Partial/ProductAssign");
        }

		[AcceptVerbs (HttpVerbs.Post)]
		public void SetProductsFilter (String categoryId)
		{
            ProductModelService.Service.SearchCriteria(String.Empty);
			Guid categoryGuidId;
			Guid.TryParse (categoryId, out categoryGuidId);
            ProductModelService.Service.SearchCriteria(FilterBy.Category, categoryGuidId);
		}

        public ActionResult CategoryDropDownLoadAsync()
        {
            List<Category> categories = Repository.Data.Get<Category>().All().ToList();
            categories.Add(new Category{ Name = "Top category", Parent = null });
            return new JsonResult { Data = new SelectList(categories.ToList(), "Id", "Name") };
        }

		[AcceptVerbs (HttpVerbs.Post)]
		[Security(Roles = UserRole.Administer)]
		public void CreateCategory( String CategoryDropDown, string Id )
		{
			var category = new Category { Parent = null };
            
			Guid parentId;
		    if(Guid.TryParse(CategoryDropDown, out parentId))
		    {
                var parentCategory = Repository.Data.Get<Category>(parentId);
                category.Parent = parentCategory;
		    }

			Guid editedId;
			if (Guid.TryParse(Id, out editedId))
			{
				category = Repository.Data.Get<Category>(editedId);
			}

			if (TryUpdateModel (category))
            {
                Repository.Data.Save(category);
            }

            ProductModelService.Service.ClearSearchCriteria();
        }

        [AcceptVerbs (HttpVerbs.Post)]
		[Security(Roles = UserRole.Administer)]
        public void DeleteCategories(string categoryId)
        {
			Guid guidCategoryId;
            if (Guid.TryParse(categoryId, out guidCategoryId))
			{
                var category = Repository.Data.Get<Category>(guidCategoryId);
                Repository.Data.Delete(category);
			}

            ProductModelService.Service.ClearSearchCriteria();
        }

       [AcceptVerbs(HttpVerbs.Post)]
	   [Security(Roles = UserRole.Administer)]
        public void AssignProductsToCategory(List<String> productIds, String category)
       {
           using (var uow = new UnitOfWork(UnitOfWorkScopeType.New))
           {
               var categoryEntity = Repository.Data.Get<Category>(Guid.Parse(category));    
           
                foreach (var productId in productIds.Distinct())
                {
                    Product product = Repository.Data.Get<Product>(Guid.Parse(productId));
                    product.Category = categoryEntity;
                    Repository.Data.Save(product);
                }

               uow.Commit();
           }
        }
    }
}
