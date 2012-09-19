using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.IO;
using System.Text;
using System.Web.Caching;
using System.Web.Compilation;
using System.Windows.Forms;

namespace Litium.Common.Resources
{
    public class ResourceProviderFactory : System.Web.Compilation.ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new ResourceProvider(classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            throw new NotSupportedException("Local resources are not supported");
        }
    }
}
