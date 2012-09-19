using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Litium.Common.Resources
{
    public class ResourceExpressionFields
    {
        private string _classKey;
        private string _resourceKey;

        internal ResourceExpressionFields(string classKey, string resourceKey)
        {
            _classKey = classKey;
            _resourceKey = resourceKey;
        }

        public string ClassKey
        {
            get
            {
                return _classKey;
            }
        }

        public string ResourceKey
        {
            get
            {
                return _resourceKey;
            }
        }
    }
}
