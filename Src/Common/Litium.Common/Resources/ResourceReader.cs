using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;

namespace Litium.Common.Resources
{
    /// <summary>
    /// Basic implementation of IResourceReader using the specified IDictionary.
    /// </summary>
    internal class ResourceReader : IResourceReader, IDisposable
    {
        private IDictionary _resources;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resources"></param>
        public ResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader"></param>
        public ResourceReader(IResourceReader reader)
        {
            _resources = new Dictionary<string, Object>();

            foreach (DictionaryEntry entry in reader)
                _resources.Add(entry.Key, entry.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary Resources
        {
            get
            {
                return _resources;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IResourceReader.Close()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        public void Dispose()
        {
            _resources = null;
            GC.SuppressFinalize(this);
        }
    }
}
