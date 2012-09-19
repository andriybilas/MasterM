using System;
using System.Collections.Generic;
using System.Reflection;
using Litium.Common;
using Litium.Common.Configurations;
using Microsoft.ApplicationServer.Caching;
using NHibernate.Cache;
using log4net;

namespace Litium.Infrastructure.DataAccess.Cache
{
	public class AppFabricCache : ICache
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IDictionary<string, DataCacheLockHandle> _lockHandles;
		private readonly string _appFabricRegionName;
		private readonly int? _expirationTimeout;

		/// <summary>
		/// Gets the name of the [NHibernate] cache region.
		/// </summary>
		public string RegionName { get; private set; }

		/// <summary>
		/// Get a reasonable "lock timeout"
		/// </summary>
		public int Timeout
		{
			get { return LitiumConfigs.Data.CacheLockTimeout; }
		}

		/// <summary>
		/// The data cache [client] for this adapter.
		/// </summary>
		private DataCache Cache { get; set; }

		/// <summary>
		/// Creates an instance of the class.
		/// </summary>
		/// <param name="regionName">The name of the NHibernate region the adapter is for.</param>
		public AppFabricCache(string regionName)
		{
			_lockHandles = new Dictionary<string, DataCacheLockHandle>();
			_appFabricRegionName = FormatRegionName(regionName);
			_expirationTimeout = LitiumConfigs.Data.CacheExpirationTimeout;
			RegionName = regionName;
			Cache = IoC.Resolve<IAppFabricCacheFactory>().GetCache(LitiumConfigs.Data.CacheName);
		}

		/// <summary>
		/// Gets the object from the cache
		/// </summary>
		/// <param name="key">Key of object to get</param>
		/// <returns>Cached object</returns>
		public object Get(object key)
		{
			if (key == null)
				return null;

			try
			{
				var value = Cache.Get(key.ToString(), _appFabricRegionName);
				return value == null ? null : AppFabricCacheOptimizer.Decompress((byte[]) value);
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
				return null;
			}
		}

		/// <summary>
		/// Caches an item.
		/// </summary>
		/// <param name="key">The key for the item to cache.</param>
		/// <param name="value">The item to cache.</param>
		public virtual void Put(object key, object value)
		{
			Put(key, value, true);
		}

		/// <summary>
		/// Caches an item. 
		/// If the region in which the item should be cached doesn't exist, it will be created.
		/// </summary>
		/// <param name="key">The key for the item to be cached.</param>
		/// <param name="value">The item to be cached.</param>
		/// <param name="createMissingRegion">A flag to determine whether or not the cache region should be created if it
		/// doesn't exist.</param>
		private void Put(object key, object value, bool createMissingRegion)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			if (value == null)
				throw new ArgumentNullException("value");

			try
			{
				var optimizedValue = AppFabricCacheOptimizer.Compress(value);
				//Entity cache is locked
				if (_lockHandles.ContainsKey(key.ToString()))
				{
					if (_expirationTimeout.HasValue)
					{
						Cache.PutAndUnlock(key.ToString(), optimizedValue, _lockHandles[key.ToString()], TimeSpan.FromMilliseconds(_expirationTimeout.Value), _appFabricRegionName);
					}
					else
					{
						Cache.PutAndUnlock(key.ToString(), optimizedValue, _lockHandles[key.ToString()], _appFabricRegionName);
					}
					lock (_lockHandles)
					{
						_lockHandles.Remove(key.ToString());
					}
				}
					//Entity cache isn't locked
				else
				{
					if (_expirationTimeout.HasValue)
					{
						Cache.Put(key.ToString(), optimizedValue, TimeSpan.FromMilliseconds(_expirationTimeout.Value), _appFabricRegionName);
					}
					else
					{
						Cache.Put(key.ToString(), optimizedValue, _appFabricRegionName);
					}
				}
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex) && createMissingRegion)
				{
					CreateRegionAndTryAgain(x => Put(key, value, false));
				}
				else if (IsNotCritical(ex) || ex.ErrorCode == DataCacheErrorCode.ObjectLocked)
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Removes an item from the cache.
		/// </summary>
		/// <param name="key">The Key of the Item in the Cache to remove.</param>
		/// <exception cref="CacheException"></exception>
		public void Remove(object key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			if (!Exists(key)) 
				return;

			try
			{
				Cache.Remove(key.ToString(), _appFabricRegionName);
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Clears the cache
		/// </summary>
		/// <exception cref="CacheException"></exception>
		public void Clear()
		{
			try
			{
				Cache.ClearRegion(_appFabricRegionName);
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Cleans up the cache.
		/// </summary>
		/// <exception cref="CacheException"></exception>
		public void Destroy()
		{
			try
			{
				Cache.RemoveRegion(_appFabricRegionName);
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Attempts to lock the key and stores a reference to the lock handle in memory.
		/// </summary>
		/// <param name="key">The key to lock.</param>
		public void Lock(object key)
		{
			if (!Exists(key)) 
				return;

			try
			{
				DataCacheLockHandle lockHandle;
				Cache.GetAndLock(key.ToString(), TimeSpan.FromMilliseconds(Timeout), out lockHandle, _appFabricRegionName);

				lock (_lockHandles)
				{
					_lockHandles.Add(key.ToString(), lockHandle);
				}
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// If this is a clustered cache, unlock the item
		/// </summary>
		/// <param name="key">The Key of the Item in the Cache to unlock.</param>
		/// <exception cref="CacheException"></exception>
		public void Unlock(object key)
		{
			if (!_lockHandles.ContainsKey(key.ToString()))
				return;

			if (!Exists(key))
				return;

			try
			{
				Cache.Unlock(key.ToString(), _lockHandles[key.ToString()], _appFabricRegionName);

				lock (_lockHandles)
				{
					_lockHandles.Remove(key.ToString());
				}
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Generates a timestamp
		/// </summary>
		/// <returns>Timestamp</returns>
		public long NextTimestamp()
		{
			return Timestamper.Next();
		}

		/// <summary>
		/// Checks whether the execption thrown by the AppFabric client is not critical. Sometime communication between
		/// the client and server can be slow for example in which case the client will throw an exception.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		private static bool IsNotCritical(DataCacheException ex)
		{
			return ex.ErrorCode == DataCacheErrorCode.ConnectionTerminated || ex.ErrorCode == DataCacheErrorCode.RetryLater ||
			       ex.ErrorCode == DataCacheErrorCode.Timeout;
		}

		/// <summary>
		/// Checks if exception is because region doesn't exist.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		private static bool IsRegionAbsent(DataCacheException ex)
		{
			return ex.ErrorCode == DataCacheErrorCode.RegionDoesNotExist;
		}

		/// <summary>
		/// Doesn't work with '.' in region name.
		/// </summary>
		private static string FormatRegionName(string regionName)
		{
			return regionName.Replace('.', '_');
		}

		private bool Exists(object key)
		{
			if (key == null)
				return false;

			try
			{
				var value = Cache.Get(key.ToString(), _appFabricRegionName);
				return value != null;
			}
			catch (DataCacheException ex)
			{
				if (IsRegionAbsent(ex))
				{
					//Skip this exception since we don't create regions for all entities by default, but only when it's required.
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
				return false;
			}
		}

		private void CreateRegionAndTryAgain(Action<bool> callback)
		{
			try
			{
				callback(Cache.CreateRegion(_appFabricRegionName));
			}
			catch (DataCacheException ex)
			{
				if (ex.ErrorCode == DataCacheErrorCode.RegionAlreadyExists)
				{
					callback(false);
				}
				else if (IsNotCritical(ex))
				{
					_log.Warn(ex.Message, ex);
				}
				else
				{
					_log.Error(ex.Message, ex);
				}
			}
		}
	}
}