using System;
using System.Data;
using System.Globalization;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Litium.Infrastructure.DataAccess.UserTypes
{
	[Serializable]
	public class CultureInfoType : IUserType
	{
		public bool IsMutable
		{
			get { return true; }
		}

		public Type ReturnedType
		{
			get { return typeof (CultureInfo); }
		}

		public SqlType[] SqlTypes
		{
			get { return new[] {new StringClobSqlType()}; }
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public object Disassemble(object value)
		{
			return value;
		}

		bool IUserType.Equals(object x, object y)
		{
			if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
				return true;

			if (ReferenceEquals(x, null))
				return false;

			return x.Equals(y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			if (names == null) throw new ArgumentNullException("names");
			if (names.Length != 1) throw new ArgumentException("You can only map to one column.", "names");

			var value = rs[names[0]] as string;
			if (value != null)
			{
				return new CultureInfo(value);
			}
			return null;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var parameter = (IDataParameter) cmd.Parameters[index];

			var v = value as CultureInfo;

			if (v == null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = v.Name;
			}
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}
	}
}