using System.Configuration;

namespace Litium.Common.Configurations
{
	/// <summary>
	/// Enacpsulates the AppFabric cache configuration section.
	/// </summary>
	public class DataSection : ConfigurationSection
	{
		private const int _defaultCacheLockTimeout = 30000;

		private const string _cacheName = "cacheName";
		private const string _cacheExpirationTimeout = "cacheExpirationTimeout";
		private const string _cacheLockTimeout = "cacheLockTimeout";
		private const string _useCache = "useCache";
		private const string _debug = "debug";
		private const string _connectionName = "connectionName";
		private const string _filesStorage = "filesStorage";
		private const string _emptyDb = "emptyDb";
		
		/// <summary>
		/// Cache name that defines cache instance for current application
		/// </summary>
		[ConfigurationProperty(_cacheName, DefaultValue = null, IsRequired = true)]
		public string CacheName
		{
			get
			{
				return (string)this[_cacheName];
			}
			set
			{
				this[_cacheName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the connection.
		/// </summary>
		/// <value>The name of the connection.</value>
		[ConfigurationProperty(_connectionName, IsRequired = true)]
		public string ConnectionName
		{
			get
			{
				return (string)this[_connectionName];
			}
			set
			{
				this[_connectionName] = value;
			}
		}

		/// <summary>
		/// Specifies the timeout in milliseconds to define when cached object expires.
		/// If isn't defined then value set on cache cluster is used (10min by default).
		/// </summary>
		[ConfigurationProperty(_cacheExpirationTimeout, DefaultValue = null, IsRequired = false)]
		public int? CacheExpirationTimeout
		{
			get
			{
				return (int?)this[_cacheExpirationTimeout];
			}
			set
			{
				this[_cacheExpirationTimeout] = value;
			}
		}

		/// <summary>
		/// Specifies the timeout in milliseconds when locking items in the cache.
		/// </summary>
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty(_cacheLockTimeout, DefaultValue = _defaultCacheLockTimeout, IsRequired = false)]
		public int CacheLockTimeout
		{
			get
			{
				return (int)this[_cacheLockTimeout];
			}
			set
			{
				this[_cacheLockTimeout] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [use cache].
		/// </summary>
		/// <value><c>true</c> if [use cache]; otherwise, <c>false</c>.</value>
		[ConfigurationProperty(_useCache, DefaultValue = true, IsRequired = false)]
		public bool UseCache
		{
			get
			{
				return (bool)this[_useCache];
			}
			set
			{
				this[_useCache] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DataSection"/> is debug.
		/// </summary>
		/// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
		[ConfigurationProperty(_debug, DefaultValue = false, IsRequired = false)]
		public bool Debug
		{
			get
			{
				return (bool)this[_debug];
			}
			set
			{
				this[_debug] = value;
			}
		}

		/// <summary>
		/// Directory path to the files storage
		/// </summary>
		[ConfigurationProperty(_filesStorage, DefaultValue = null, IsRequired = true)]
		public string FilesStorage
		{
			get
			{
				return (string)this[_filesStorage];
			}
			set
			{
				this[_filesStorage] = value;
			}
		}

		/// <summary>
		/// Defines if DB should be cleaned from data when application starts.
		/// (true for tests)
		/// </summary>
		[ConfigurationProperty(_emptyDb, DefaultValue = false, IsRequired = false)]
		public bool EmptyDb
		{
			get
			{
				return (bool)this[_emptyDb];
			}
			set
			{
				this[_emptyDb] = value;
			}
		}
	}
}
