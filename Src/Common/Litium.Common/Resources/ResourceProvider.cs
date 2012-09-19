using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web.Caching;
using System.Web.Compilation;
using Litium.Common.Configurations;

namespace Litium.Common.Resources
{
    /// <summary>
    /// Custom *.resx provider.
    /// </summary>
    internal class ResourceProvider : IResourceProvider
    {
        /// <summary>
        /// Specified the default culture. 
        /// </summary>
        private static readonly CultureInfo _defaultUICulture = CultureInfo.GetCultureInfoByIetfLanguageTag(LitiumConfigs.Globalization.DefaultResourceCulture);

        private string _classKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="classKey">Name of the resource.</param>
        internal ResourceProvider(string classKey)
        {
            _classKey = classKey;
        }

        /// <summary>
        /// Path of the resource file.
        /// </summary>
        public string Path
        {
            get
            {
                return string.Format(@"~/{0}{1}.resx",
                                     LitiumConfigs.Globalization.DefaultResourceFolder,
                                     _classKey);
            }
        }

        #region IResourceProvider Members

        /// <summary>
        /// Returns the resource object. It returns string.Empty instead of null since this provider
        /// is implemented to handle string resources.
        /// </summary>
        /// <param name="resourceKey">Key of the resource.</param>
        /// <param name="culture">Culture of the resource.</param>
        /// <returns>Returns the resource object.</returns>
        /// <remarks>Returns string.Empty instead of null when the resource object is missing.</remarks>
        public object GetObject(string resourceKey, CultureInfo culture)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture;

            // Optimization: avoid lookups for the _defaultUICulture.
            if (culture.Equals(_defaultUICulture))
                culture = null;

            var reader = GetResourceCache(culture) as ResourceReader;

            if (reader != null && reader.Resources.Contains(resourceKey))
                return reader.Resources[resourceKey];
            
            // If key wasn't found for the culture specified, retry the default culture.
            if (culture != null)
                return GetObject(resourceKey, _defaultUICulture);

            return string.Empty;
        }

        /// <summary>
        /// Resource reader.
        /// </summary>
        public IResourceReader ResourceReader
        {
            get
            {
                return GetResourceCache(null);
            }
        }
        
        #endregion

        /// <summary>
        /// Returns the resource item from the cache.
        /// </summary>
        /// <param name="culture">Culture of the resource item.</param>
        /// <returns>Resource item from the cache.</returns>
        private IResourceReader GetResourceCache(CultureInfo culture)
        {
            Cache cache = null;

            // Check the HttpContext current in order to avoid compilation errors
            if (System.Web.HttpContext.Current != null)
                cache = System.Web.HttpContext.Current.Cache;
            IResourceReader resourceReader;
            string fullPath = Resolve(Path, culture), key = "LResourceFile " + fullPath;

            if (null == (resourceReader = cache != null ? cache[key] as IResourceReader : null))
            {
                bool exists = File.Exists(fullPath);
                resourceReader = exists ? new ResourceReader(new ResXResourceReader(fullPath)) : new ResourceReader(new Dictionary<string, object>());
                if (cache != null)
                {
                    cache.Insert(key,
                                 resourceReader,
                                 exists ? new CacheDependency(fullPath) : null,
                                 Cache.NoAbsoluteExpiration,
                                 Cache.NoSlidingExpiration,
                                 CacheItemPriority.NotRemovable,
                                 new CacheItemRemovedCallback(this.OnRemoved));
                }
            }

            return resourceReader;
        }

        /// <summary>
        /// CacheItemRemovedCallback to properly dispose of IDisposables.
        /// </summary>
        /// <param name="key">Key of the resource item in the cache.</param>
        /// <param name="value">Value of the resource item in the cache.</param>
        /// <param name="reason">Why the resource item is removed from the cache.</param>
        public void OnRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            var disposable = value as IDisposable;
            if (null != disposable)
                disposable.Dispose();

            value = null;
        }

        /// <summary>
        /// Returns the absolute path to the *.resx corresponding to the requested culture.
        /// </summary>
        /// <param name="path">Path to the default *.resx (without culture info in the file name).</param>
        /// <param name="culture">Requested culture.</param>
        /// <returns>Absolute path to the *.resx corresponding to the requested culture.</returns>
        private static string Resolve(string path,
                                      CultureInfo culture)
        {
            if (culture != null && path != null)
            {
                var fileName = System.IO.Path.GetFileName(path);
                if (fileName != null)
                {
                    path = path.Replace(fileName,
                                        string.Format("{0}.{1}{2}",
                                                      System.IO.Path.GetFileNameWithoutExtension(path),
                                                      culture.Name,
                                                      System.IO.Path.GetExtension(path)));
                }
            }
            // Check the HttpContext current in order to avoid compilation errors
            if (System.Web.HttpContext.Current != null)
                return System.Web.HttpContext.Current.Server.MapPath(path);
            
            return GetIoPath(path);
        }

        private static string GetIoPath(string path)
        {
            if (path != null)
            {
                if (path.IndexOf("~/") > -1)
                    path = path.Substring(path.IndexOf("~/") + 2);
                path = path.Replace("/", "\\");
                string fullPath = System.IO.Path.GetFullPath("~");
                if (fullPath.IndexOf("bin") > -1)
                    fullPath = fullPath.Remove(fullPath.IndexOf("bin"));
                return fullPath + path;
            }
            return path;
        }
    }

}
