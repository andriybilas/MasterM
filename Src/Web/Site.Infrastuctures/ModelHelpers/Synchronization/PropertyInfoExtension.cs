using System;
using System.Reflection;

namespace Site.Infrastuctures.ModelHelpers.Synchronization
{
    public static class  PropertyInfoExtension
    {
        public static String GetNullValue (this PropertyInfo propertyInfo, object target)
        {
            return propertyInfo.GetValue(target, null) == null ? "NULL" : propertyInfo.GetValue(target, null).ToString();
        }

        public static bool InvariantEquals ( this String source, String campare )
        {
            return source.Equals(campare, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}