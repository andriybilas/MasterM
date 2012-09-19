using System;
using System.Collections.Generic;
using NHibernate.Cache;

namespace Litium.Infrastructure.DataAccess.Cache
{
	public class AppFabricCacheProvider : ICacheProvider
	{
		public ICache BuildCache(string regionName, IDictionary<string, string> properties)
		{
			if (string.IsNullOrEmpty(regionName))
				throw new ArgumentNullException("regionName");

			return new AppFabricCache(regionName);
		}

		public long NextTimestamp()
		{
			return Timestamper.Next();
		}

		public void Start(IDictionary<string, string> properties)
		{
		}

		public void Stop()
		{
		}
	}
}
