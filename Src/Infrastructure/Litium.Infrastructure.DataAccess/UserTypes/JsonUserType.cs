using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Newtonsoft.Json;

namespace Litium.Infrastructure.DataAccess.UserTypes
{
	public class JsonUserType<T> : IUserType
	{
		// ReSharper disable StaticFieldInGenericType
		private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
		                                                                         	{
		                                                                         		TypeNameHandling = TypeNameHandling.Auto, CheckAdditionalContent = true
		                                                                         	};

		// ReSharper restore StaticFieldInGenericType

		#region IUserType Members

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
			return false; //TODO: Handle with deep copying?
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public bool IsMutable
		{
			get { return true; }
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			if (names == null) throw new ArgumentNullException("names");
			if (names.Length != 1) throw new ArgumentException(@"You can only map to one column.", "names");

			var value = rs[names[0]] as string;
			if (value != null)
			{
				return JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
			}
			return null;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var parameter = (IDataParameter) cmd.Parameters[index];

			string json = JsonConvert.SerializeObject(value, Formatting.None, _jsonSerializerSettings);
			parameter.Value = json;
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public Type ReturnedType
		{
			get { return typeof (T); }
		}

		public SqlType[] SqlTypes
		{
			get { return new SqlType[] { new StringClobSqlType() }; }
		}

		#endregion
	}
}