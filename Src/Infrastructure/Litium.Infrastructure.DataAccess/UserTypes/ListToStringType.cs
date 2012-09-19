using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Litium.Infrastructure.DataAccess.UserTypes
{
	[Serializable]
	public class ListToStringType : IUserType
	{
		public bool IsMutable
		{
			get { return false; }
		}

		public Type ReturnedType
		{
			get { return typeof (List<string>); }
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
			if (ReferenceEquals(x, y))
				return true;

			if (x != null)
				return x.Equals(y);

			return false;
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
				return value.Split(',').ToList();
			}
			return null;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var parameter = (IDataParameter) cmd.Parameters[index];

			if (value == null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = string.Join(",", ((IEnumerable<string>) value).ToArray());
			}
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}
	}
}