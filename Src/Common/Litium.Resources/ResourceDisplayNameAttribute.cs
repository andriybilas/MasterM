using System.ComponentModel;

namespace Litium.Resources
{
    public class ResourceDisplayNameAttribute : DisplayNameAttribute
    {
        public ResourceDisplayNameAttribute(string resourceKey) : base(GetMessageFromResource(resourceKey))
        {
            
        }

        private static string GetMessageFromResource(string key)
        {
            return StoreResourceStrings.Get(key);
        }
    }
}
