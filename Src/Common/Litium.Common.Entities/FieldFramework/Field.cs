using System;
using System.Text;

namespace Litium.Common.Entities.FieldFramework
{
    [Serializable]
    public class Field
    {
        private readonly string _name;
        private readonly object _value;

        public Field(string name, object value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            if (Name != null)
            {
                builder.Append(Name);
            }
            builder.Append(", ");
            if (Value != null)
            {
                builder.Append(Value.ToString());
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}
