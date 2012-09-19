using System;
using System.Threading;

namespace Litium.Resources
{
	public class StoreResourceStrings : ResourceStrings<WebStroreResource>
	{
		private static StoreResourceStrings _instance;

		public static string Get ( String key )
		{
			if (_instance == null) _instance = new StoreResourceStrings ();
			return _manager.GetString (key, Thread.CurrentThread.CurrentUICulture) ?? string.Format("[Error : No resource for such key {0}]", key);
		}
		private StoreResourceStrings () : base ("Litium.Resources.WebStroreResource") { }

		#region Resource strings

		//Users
        public static String LoginName { get { return Get (ResourceKey.LoginName); } }
		public static string LastLogin { get { return Get (ResourceKey.LastLogin); } }
        public static string UserDetails { get { return Get(ResourceKey.UserDetails); } }
		public static string Address { get { return Get (ResourceKey.Address); } }
		public static string NoGridRecords { get { return Get (ResourceKey.NoGridRecords); } }
		public static string InvalidUserNameOrPassword { get { return Get (ResourceKey.InvalidUserNameOrPassword); } }
		public static string Users { get { return Get(ResourceKey.Users); } }
		public static string UserInfo { get { return Get(ResourceKey.UserInfo); } }
		public static string Address1 { get { return Get(ResourceKey.Address1); } }
		public static string Address2 { get { return Get(ResourceKey.Address2); } }
		public static string City { get { return Get(ResourceKey.City); } }
        public static string Email { get { return Get(ResourceKey.Email); } }
        public static string FirstName { get { return Get(ResourceKey.FirstName); } }
        public static string LastName { get { return Get(ResourceKey.LastName); } }
        public static string MiddleName { get { return Get(ResourceKey.MiddleName); } }
        public static string Phone { get { return Get(ResourceKey.Phone); } }
        public static string PhoneHome { get { return Get(ResourceKey.PhoneHome); } }
        public static string PhoneMobile { get { return Get(ResourceKey.PhoneMobile); } }
        public static string PIB { get { return Get(ResourceKey.PIB); } }
		public static string LoginFailed { get { return Get(ResourceKey.LoginFailed); } }

        //Products
        public static string Products { get { return Get(ResourceKey.Products); } }
		public static string ProductBrend { get { return Get (ResourceKey.ProductBrend); } }
		public static string ProductCapacity { get { return Get (ResourceKey.ProductCapacity); } }
		public static string ProductCountry { get { return Get (ResourceKey.ProductCountry); } }
		public static string ProductWeight { get { return Get (ResourceKey.ProductWeight); } }
		public static string UpdateDate { get { return Get (ResourceKey.UpdateDate); } }
		public static string CreateDate { get { return Get (ResourceKey.CreateDate); } }
		public static string Published { get { return Get (ResourceKey.Published); } }
		public static string StockBalance { get { return Get (ResourceKey.StockBalance); } }
		public static string Price { get { return Get (ResourceKey.Price); } }
		public static string ProductName { get { return Get (ResourceKey.ProductName); } }
		public static string OnlyImagesIsAllovedToUpload { get { return Get (ResourceKey.OnlyImagesIsAllovedToUpload); } }
		public static string AssignToCategory { get { return Get (ResourceKey.AssignToCategory); } }
		public const string PublicUploadFolderPath = @"~/PublicFiles/ProductImages";

		//Category
		public static string Category { get { return Get(ResourceKey.Category); } }
		public static string CreateNewCategory { get { return Get(ResourceKey.CreateNewCategory); } }
		public static string Create { get { return Get(ResourceKey.Create); } }
		public static string DeleteCategory { get { return Get(ResourceKey.DeleteCategory); } }
        public static string DeleteCategoryConfirm { get { return Get(ResourceKey.DeleteCategoryConfirm); } }
        public static string CategoryNull { get { return Get(ResourceKey.CategoryNull); } }
		public static string PublishConfirmation { get { return Get (ResourceKey.PublishConfirmation); } }
		public static string CategoryGridFilter { get { return Get (ResourceKey.CategoryGridFilter); } }
		public static string CategoryFilter { get { return Get (ResourceKey.CategoryFilter); } }
		public static string All { get { return Get (ResourceKey.All); } }
		public static string Uncategorized { get { return Get (ResourceKey.Uncategorized); } }
		public static string UncategorizedId { get { return Get (ResourceKey.UncategorizedId); } }
		public static string ProductNameFilter { get { return Get (ResourceKey.ProductNameFilter); } }
		public static string ProductCategoryFilter { get { return Get (ResourceKey.ProductCategoryFilter); } }
        public static string TopCategory { get { return Get(ResourceKey.TopCategory); } }
        public static string EditCategory { get { return Get(ResourceKey.EditCategory); } }

        //Order
        public static string Order { get { return Get (ResourceKey.Order); } }
        public static string OrderDetails { get { return Get(ResourceKey.OrderDetails); } }
		public static string NullOrderNumberText { get { return Get(ResourceKey.NullOrderNumberText); } }
		

		//ProductSet
		public static string ProductSetHeader { get { return Get (ResourceKey.ProductSetHeader); } }
		public static string CurrentProductSets { get { return Get (ResourceKey.CurrentProductSets); } }
		public static string AllProducts { get { return Get (ResourceKey.AllProducts); } }
		public static string CreateNewProductSet { get { return Get (ResourceKey.CreateNewProductSet); } }
		public static string Search { get { return Get (ResourceKey.Search); } }
		public static string ProductFilter { get { return Get (ResourceKey.ProductFilter); } }
		public static string DeleteProductSet { get { return Get (ResourceKey.DeleteProductSet); } }
		public static string DeleteProductSetConfirm { get { return Get (ResourceKey.DeleteProductSetConfirm); } }
        public static string AssignProductSetComplete { get { return Get(ResourceKey.AssignProductSetComplete); } }

		//Campaign
		public static string Campaign { get { return Get (ResourceKey.Campaign); } }
		public static string DeleteCampaignConfirm { get { return Get (ResourceKey.DeleteCampaignConfirm); } }
		public static string CampaignHeader { get { return Get (ResourceKey.CampaignHeader); } }
		public static string CreateNewCampaign { get { return Get (ResourceKey.CreateNewCampaign); } }
		public static string DeleteCampaign { get { return Get (ResourceKey.DeleteCampaign); } }
		public static string CurrentCampaign { get { return Get (ResourceKey.CurrentCampaign); } }
		public static string AllCampaign { get { return Get (ResourceKey.AllCampaign); } }
		public static string Select { get { return Get (ResourceKey.Select); } }
		public static string CampaignTypes { get { return Get (ResourceKey.CampaignTypes); } }
		public static string SelectProductCampaign { get { return Get (ResourceKey.SelectProductCampaign); } }
		public static string Add { get { return Get (ResourceKey.Add); } }
		public static string AddProducts { get { return Get (ResourceKey.AddProducts); } }
        public static string ChouseProductsToAssign { get { return Get(ResourceKey.ChouseProductsToAssign); } }
        public static string UpdateCampaign { get { return Get(ResourceKey.UpdateCampaign); } }

	    #endregion Resource strings
	}
}
