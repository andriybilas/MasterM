using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace Litium.Infrastructure.DataAccess.LinqFunctions
{
	public class LitiumEqualsGenerator : BaseHqlGeneratorForMethod
	{
		public LitiumEqualsGenerator()
		{
			SupportedMethods = new[]
			                   	{
			                   		ReflectionHelper.GetMethodDefinition<int>(x => x.Equals(default(int))),
			                   		ReflectionHelper.GetMethodDefinition<short>(x => x.Equals(default(short))),
			                   		ReflectionHelper.GetMethodDefinition<long>(x => x.Equals(default(long))),
			                   		ReflectionHelper.GetMethodDefinition<DateTime>(x => x.Equals(default(DateTime))),
			                   		ReflectionHelper.GetMethodDefinition<Guid>(x => x.Equals(default(Guid))),
			                   		ReflectionHelper.GetMethodDefinition<double>(x => x.Equals(default(double))),
			                   		ReflectionHelper.GetMethodDefinition<float>(x => x.Equals(default(float))),
			                   		ReflectionHelper.GetMethodDefinition<decimal>(x => x.Equals(default(decimal))),
			                   		ReflectionHelper.GetMethodDefinition<char>(x => x.Equals(default(char))),
									ReflectionHelper.GetMethodDefinition<String>(x => x.Equals(default(string), StringComparison.InvariantCultureIgnoreCase))
			                   	};
		}

		public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
		{
			return treeBuilder.Equality(
				visitor.Visit(targetObject).AsExpression(),
				visitor.Visit(arguments[0]).AsExpression());
		}
	}

	public class LitiumBoolEqualsGenerator : BaseHqlGeneratorForMethod
	{
		public LitiumBoolEqualsGenerator()
		{
			SupportedMethods = new[]
			                   	{
			                   		ReflectionHelper.GetMethodDefinition<bool>(x => x.Equals(default(bool)))
			                   	};
		}

		public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
		{
			// HqlGeneratorExpressionTreeVisitor.VisitConstantExpression will always return an HqlEquality 
			// instead of HqlParameter for argument that is of type bool.
			// Use the HqlParameter that exists as first children to the HqlEquality as second argument into treeBuilder.Equality
			return treeBuilder.Equality(
				visitor.Visit(targetObject).AsExpression(),
				visitor.Visit(arguments[0]).Children.First().AsExpression());
		}
	}
}