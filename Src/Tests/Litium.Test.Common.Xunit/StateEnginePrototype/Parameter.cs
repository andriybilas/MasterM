using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Litium.Test.Common.Xunit.StateEnginePrototype
{
	/// <summary>
	/// 	A serializable Enum class.
	/// </summary>
	[Serializable]
	public abstract class Parameter<T> : IObjectReference
	{
		private readonly string _name;
		internal static IDictionary<string, Parameter<T>> AllParameters = new Dictionary<string, Parameter<T>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Parameter&lt;T&gt;"/> class.
		/// </summary>
		private Parameter()
		{
			// typesafe enum pattern, no public constructor
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Parameter&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		protected internal Parameter(string name)
		{
			// typesafe enum pattern, no public constructor
			_name = name;
			var key = MakeKey(name);

			if (AllParameters.ContainsKey(key))
			{
				throw new ArgumentException("Parameter name " + key + " already used!");
			}

			AllParameters[key] = this;
		}

		/// <summary>
		/// Returns the real object that should be deserialized, rather than the object that the serialized stream specifies.
		/// </summary>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> from which the current object is deserialized.</param>
		/// <returns>
		/// Returns the actual object that is put into the graph.
		/// </returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. The call will not work on a medium trusted server.</exception>
		public object GetRealObject(StreamingContext context)
		{
			return ReadResolve();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return _name;
		}

		/// <summary>
		/// 	Resolves the deserialized instance to the local reference for accurate equals() and == comparisons.
		/// </summary>
		/// <returns> a reference to Parameter as resolved in the local VM </returns>
		/// <throws>IOException</throws>
		protected internal virtual object ReadResolve()
		{
			Parameter<T> value;
			if (!AllParameters.TryGetValue(MakeKey(_name), out value))
			{
				throw new IOException("Unknown parameter value: " + _name);
			}
			return value;
		}

		protected static string MakeKey(string name)
		{
			return typeof(T) + " " + name;
		}
	}
}
