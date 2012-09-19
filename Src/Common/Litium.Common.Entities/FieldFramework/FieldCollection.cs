using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Litium.Common.Entities.FieldFramework
{
    [Serializable]
    public class FieldCollection : DynamicObject, ICollection<Field>, ISerializable
    {
        private readonly InnerFieldCollection _inner = new InnerFieldCollection();

        public FieldCollection()
        {
        }

        protected FieldCollection(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                _inner.Add((Field)entry.Value);
            }
        }

        #region Wrapping of _inner collection to implement ICollection<Field>

        public IEnumerator<Field> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<Field>.Add(Field item)
        {
            int countBefore = Count;
            _inner.Add(item);
            Contract.Assume(Count >= countBefore);
        }

        public void Clear()
        {
            _inner.Clear();

            Contract.Assume(Count == 0);
        }

        public bool Contains(Field item)
        {
            bool result = _inner.Contains(item);
            Contract.Assume(!result || Count > 0);
            return result;
        }

        public void CopyTo(Field[] array, int arrayIndex)
        {
            Contract.Assume(arrayIndex + Count <= array.Length);
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(Field item)
        {
			return _inner.Remove(item);
        }

        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == _inner.Count);
                return _inner.Count;
            }
        }

        bool ICollection<Field>.IsReadOnly
        {
            get { return ((ICollection<Field>)_inner).IsReadOnly; }
        }

        #endregion

        public bool Contains(string key)
        {
            return _inner.Contains(key);
        }

        public void Add(string key, object value)
        {
            _inner.Add(new Field(key, value));
        }

		public void Remove(string key)
		{
			_inner.Remove(key);
		}

    	public object this[string key]
        {
            get
            {
                Contract.Requires(key != null);
                var field = _inner.TryGetField(key);
                return field != null ? field.Value : null;
            }
            set
            {
                if (_inner.Contains(key))
                {
                    _inner.Remove(key);
                }
                Add(key, value);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Contract.Assume(binder != null);
            Contract.Assume(binder.Name != null);

            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;

            return true;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (Field field in _inner)
            {
                Contract.Assume(field.Name != null);
                info.AddValue(field.Name, field);
            }
        }
    }
}
