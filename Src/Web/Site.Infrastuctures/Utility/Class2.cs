using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using Litium.Resources;

namespace Site.Infrastuctures.Utility
{
	public class Columns
	{
		private static ResourceManager _manager;
		private static Columns _instance;
		
		private Columns()
		{
			Assembly assembly = typeof (EShopResource).Assembly;
            _manager = new ResourceManager("Litium.Resources.EShopResource", assembly);
		}

		public static String LoginName
		{
			get { return Get("ColLoginName"); }
		}

		public static string LastLogin { get { return Get ("ColLastLogin"); } }

		public static string Address { get { return Get ("ColAddress"); } }

		public static string EmptyGridResult { get { return Get("EmptyGridResult"); } }

		private static string Get(String key)
		{
			if (_instance == null) _instance = new Columns();
			return _manager.GetString(key, Thread.CurrentThread.CurrentUICulture);
		}
	}
}
