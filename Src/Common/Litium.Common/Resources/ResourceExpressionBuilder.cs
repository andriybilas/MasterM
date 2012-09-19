using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using Litium.Common.Configurations;

namespace Litium.Common.Resources
{
    /// <summary>
    /// Expression builder support for $ LResources expressions.
    /// </summary>
    public class ResourceExpressionBuilder : ExpressionBuilder
    {
        private static ResourceProviderFactory _resourceProviderFactory;

        public ResourceExpressionBuilder() {}

        /// <summary>
        /// Extension to get the resource object for CurrentUICulture.
        /// </summary>
        /// <param name="classKey">Key for the resource class.</param>
        /// <param name="resourceKey">Key for the resource.</param>
        /// <returns>Resource object from the resources for CurrentUICulture.</returns>
        public static object GetGlobalResourceObject(string classKey, string resourceKey)
        {
            return GetGlobalResourceObject(classKey, resourceKey, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Extension to get the resource object with a specific culture.
        /// </summary>
        /// <param name="classKey">Key for the resource class.</param>
        /// <param name="resourceKey">Key for the resource.</param>
        /// <param name="culture">Specific culture.</param>
        /// <returns>Resource object from the resources for a specific culture.</returns>
        public static object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
        {
            EnsureResourceProviderFactory();
            IResourceProvider provider = _resourceProviderFactory.CreateGlobalResourceProvider(classKey);
            return provider.GetObject(resourceKey, culture);
        }

        /// <summary>
        /// Extension to get the resource object as string for CurrentUICulture and default resource for web strings.
        /// </summary>
        /// <param name="resourceKey">Key for the resource.</param>
        /// <returns>String from the resources for CurrentUICulture and default resource for web strings.</returns>
        public static string GetWebString(string resourceKey)
        {
            return GetGlobalResourceObject(LitiumConfigs.Globalization.DefaultResourceWebStrings, resourceKey) as string;
        }

        public override object EvaluateExpression(object target, BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
        {
            var fields = parsedData as ResourceExpressionFields;
            EnsureResourceProviderFactory();
            IResourceProvider provider = _resourceProviderFactory.CreateGlobalResourceProvider(fields.ClassKey);
            return provider.GetObject(fields.ResourceKey, null);
        }

        public override System.CodeDom.CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
        {
            var fields = parsedData as ResourceExpressionFields;

            var exp = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ResourceExpressionBuilder)), "GetGlobalResourceObject", new CodePrimitiveExpression(fields.ClassKey), new CodePrimitiveExpression(fields.ResourceKey));

            return exp;
        }

        public override object ParseExpression(string expression, Type propertyType, ExpressionBuilderContext context)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentException("Too few parameters");
            }

            ResourceExpressionFields fields = null;
            string classKey = null;
            string resourceKey = null;

            string[] expParams = expression.Split(new char[] { ',' });
            if (expParams.Length > 2)
            {
                throw new ArgumentException("Too many parameters");
            }
            if (expParams.Length == 1)
            {
                throw new ArgumentException("Too few parameters");
            }
            else
            {
                classKey = expParams[0].Trim();
                resourceKey = expParams[1].Trim();
            }

            fields = new ResourceExpressionFields(classKey, resourceKey);

            EnsureResourceProviderFactory();
            IResourceProvider rp = _resourceProviderFactory.CreateGlobalResourceProvider(fields.ClassKey);

            object res = rp.GetObject(fields.ResourceKey, CultureInfo.InvariantCulture);
            if (res == null)
            {
                throw new ArgumentException("Resource not found");
            }
            return fields;
        }

        /// <summary>
        /// Ensures that there is a resource provider factory.
        /// </summary>
        private static void EnsureResourceProviderFactory()
        {
            if (_resourceProviderFactory == null)
            {
                _resourceProviderFactory = new ResourceProviderFactory();
            }
        }

        /// <summary>
        /// Whether the expression builder supports the express evaluation or not.
        /// </summary>
        public override bool SupportsEvaluate
        {
            get { return true; }
        }
    }
}
