using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Litium.Common.Entities.FieldFramework
{
    internal class InnerFieldCollection : KeyedCollection<string, Field>
    {
        protected override string GetKeyForItem(Field item)
        {
            Contract.Assume(item != null);
            return item.Name;
        }

        public Field TryGetField(string key)
        {
            Contract.Requires(key != null);

            if (Dictionary == null) return null;

            Field field;
            if (Dictionary.TryGetValue(key, out field))
            {
                return field;
            }

            return null;
        }
    }
}
