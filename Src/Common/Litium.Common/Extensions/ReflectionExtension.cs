using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Litium.Common.Extensions
{
	public static class ReflectionExtension
	{
		/// <summary>
		/// 	Converts the specified method info to another generic type.
		/// </summary>
		/// <param name = "method">The method.</param>
		/// <param name = "declaringTypeArguments">The declaring type arguments.</param>
		/// <returns></returns>
		public static MethodInfo Convert(this MethodInfo method, params Type[] declaringTypeArguments)
		{
			var baseType = method.DeclaringType.GetGenericTypeDefinition().MakeGenericType(declaringTypeArguments);
			return MethodBase.GetMethodFromHandle(method.MethodHandle, baseType.TypeHandle) as MethodInfo;
		}

		/// <summary>
		/// 	Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
		public static MethodInfo GetMethodInfo(this object obj, Expression<Action> expression)
		{
			return obj.GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// 	Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
		public static MethodInfo GetMethodInfo<T>(this object obj, Expression<Action<T>> expression)
		{
			return obj.GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// 	Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
		public static MethodInfo GetMethodInfo<T, TResult>(this object obj, Expression<Func<T, TResult>> expression)
		{
			return obj.GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// 	Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
		public static MethodInfo GetMethodInfo(this object obj, LambdaExpression expression)
		{
			MethodCallExpression outermostExpression = expression.Body as MethodCallExpression;

			if (outermostExpression == null)
			{
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			}

			return outermostExpression.Method;
		}

		/// <summary>
		/// 	Gets the property info.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <param name = "obj">The obj.</param>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
		public static PropertyInfo GetPropertyInfo<T>(this T obj, Expression<Func<T, object>> expression)
		{
			return GetPropertyInfo(expression);
		}

		/// <summary>
		/// 	Gets the property info.
		/// </summary>
		/// <param name = "expression">The expression.</param>
		/// <returns></returns>
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