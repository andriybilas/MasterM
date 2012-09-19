using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Litium.Infrastructure.DataAccess.Conventions
{
	public class SchemaTableNameConvention : IClassConvention
	{
		public void Apply(IClassInstance instance)
		{
			//instance.Schema(GetSchemaName(instance.EntityType.Namespace));
			instance.Table(GetSchemaName(instance.EntityType.Namespace, instance.EntityType.Name));
		}

		private string GetSchemaName(string ns, string name)
		{
			if (string.IsNullOrEmpty(ns))
				return name;

			var parts = ns.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return name;

			var lastPart = parts[parts.Length - 1];
			if (lastPart == "Entities")
				return name;

			return lastPart + "_" + name;
		}
	}
}