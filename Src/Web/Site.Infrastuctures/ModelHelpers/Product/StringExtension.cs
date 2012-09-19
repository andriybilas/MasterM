using System;
using System.Text.RegularExpressions;

namespace Site.Infrastuctures.ModelHelpers.Product
{

	public static class StringExtension
	{
		public static bool ContainsIgnoreKey ( this String source, String compareString)
		{
			return Regex.Match (source, compareString, RegexOptions.IgnoreCase).Success;
		}
	}
}