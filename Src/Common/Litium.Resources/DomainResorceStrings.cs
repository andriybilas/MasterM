using System;
using System.Threading;

namespace Litium.Resources
{
	public class DomainResorceStrings : ResourceStrings<DomainNotification>
	{
		private static DomainResorceStrings _instance;

		private DomainResorceStrings () : base("Litium.Resources.DomainNotification") { }

		private static string Get ( String key )
		{
			if (_instance == null) _instance = new DomainResorceStrings ();
			return _manager.GetString (key, Thread.CurrentThread.CurrentUICulture);
		}
	}
}
