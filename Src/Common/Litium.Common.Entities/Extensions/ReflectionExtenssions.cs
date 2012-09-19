using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Litium.Common.Entities.Extensions
{
	public static class ReflectionExtensions
	{
		public static PropertyInfo GetPropertyInfo<T>(this T obj, Expression<Func<T, object>> expression)
		{
			return GetPropertyInfo(expression);
		}

		public static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
		{
			var lambda = expression;
			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = lambda.Body as UnaryExpression;
				memberExpression = unaryExpression.Operand as MemberExpression;
			}
			else
			{
				memberExpression = lambda.Body as MemberExpression;
			}
			return memberExpression == null ? null : memberExpression.Member as PropertyInfo;
		}
	}
}