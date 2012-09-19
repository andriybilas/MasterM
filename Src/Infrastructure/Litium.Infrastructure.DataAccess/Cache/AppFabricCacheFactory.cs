using System;
using System.Reflection;
using Microsoft.ApplicationServer.Caching;
using log4net;

namespace Litium.Infrastructure.DataAccess.Cache
{
	public class AppFabricCacheFactory : IDisposable, IAppFabricCacheFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly DataCacheFactory _cacheCluster;

		/// <summary>
		/// A lazy thread-safe singleton implemnatation due to the cost of creating <see cref="DataCacheFactory"/>.
		/// </summary>
		public AppFabricCacheFactory()
		{
			_cacheCluster = new DataCacheFactory();
		}

		/// <summary>
		/// Gets an instance of a data cache [client].
		/// </summary>
		/// <param name="cacheName">The name of the AppFabric cache to get the data cache [client] for.</param>
		/// <returns>A data cache [client].</returns>
		public DataCache GetCache(string cacheName)
		{
			try
			{
				return _cacheCluster.GetCache(cacheName);
			}
			catch (DataCacheException ex)
			{
				_log.Error(ex.Message, ex);
				throw;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_cacheCluster.Dispose();
		}
	}
}
