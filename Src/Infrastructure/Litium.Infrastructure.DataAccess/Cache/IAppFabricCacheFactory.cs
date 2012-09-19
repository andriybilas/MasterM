using Microsoft.ApplicationServer.Caching;

namespace Litium.Infrastructure.DataAccess.Cache
{
	public interface IAppFabricCacheFactory
	{
		/// <summary>
		/// Gets an instance of a data cache [client].
		/// </summary>
		/// <param name="cacheName">The name of the AppFabric cache to get the data cache [client] for.</param>
		/// <returns>A data cache [client].</returns>
		DataCache GetCache(string cacheName);
	}
}