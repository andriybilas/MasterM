using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Litium.Common.Configurations
{
	/// <summary>
	/// Plugin configuratino section
	/// </summary>
	public class PluginsSection : ConfigurationSection
	{
		private readonly List<string> _assembliesAdded = new List<string>();
		private readonly List<string> _assembliesRemoved = new List<string>();
		private IEnumerable<Assembly> _solutionAssemblies;

		/// <summary>
		/// Gets the name of assemblies to load first.
		/// </summary>
		/// <value>The assemblies.</value>
		public IEnumerable<string> Assemblies
		{
			get { return _assembliesAdded.AsReadOnly(); }
		}

		/// <summary>
		/// Gets the solution assemblies.
		/// </summary>
		/// <value>The solution assemblies.</value>
		public IEnumerable<Assembly> SolutionAssemblies
		{
			get { return _solutionAssemblies ?? (_solutionAssemblies = GetSolutionAssemblies()); }
		}

		/// <summary>
		/// Get the solution assemblies in correct order for parsing.
		/// </summary>
		private IEnumerable<Assembly> GetSolutionAssemblies()
		{
			var lastAssemblieNames = new[]
				{
					"Litium.Common",
					"Litium.Common.Entities",
					"Litium.Common.Setup",
					"Litium.Domain",
					"Litium.Domain.Entities",
					"Litium.Domain.Mappings",
					"Litium.Domain.WebControls",
					"Litium.Domain.Web"
				};

			var ignoreAssamblieNames = new[]
				{
					"Anonymously Hosted DynamicMethods Assembly",
					"aspNetEmail",
					"Castle.Core",
					"Castle.Windsor",
					"ComponentArt.Charting.WebChart",
					"ComponentArt.Web.UI",
					"CookComputing.XmlRpc",
					"Ionic.Utils.Zip",
					"Litium.Framework.Cache",
					"Litium.Framework.Search",
					"Litium.Framework.Search.Indexing",
					"Litium.Framework.Search.Indexing.Lucene",
					"Litium.Framework.Search.Lucene",
					"Litium.Studio.HtmlEditor",
					"Litium.Utilities",
					"Litium.WebControls.GUI",
					"log4net",
					"Lucene.Net",
					"Microsoft.Practices.EnterpriseLibrary.Caching",
					"Microsoft.Practices.EnterpriseLibrary.Common",
					"Microsoft.Practices.ObjectBuilder2",
					"Microsoft.Practices.Unity",
					"NetSpell.SpellChecker",
					"paypal_base",
					"Telerik.Web.UI",
					"nunit.core",
					"nunit.core.interfaces",
					"nunit.framework",
					"nunit.tdnet",
					"nunit.util",
					"Newtonsoft.Json",
					"TestDriven.Framework",
					"xunit",
					"xunit.extensions",
					"FluentNHibernate",
					"NHibernate",
					"NHibernate.ByteCode.Castle",
					"Iesi.Collections",
					"Litium.Xunit",
					"HibernatingRhinos.Profiler.Appender.v4.0",
					"TestDriven.TestRunner"
				}
				.Concat(_assembliesRemoved.FindAll(a =>
													lastAssemblieNames.FirstOrDefault(l =>
																					  l.Equals(a, StringComparison.InvariantCultureIgnoreCase)) == null))
				.Distinct();

			var firstAssamblieNames = _assembliesAdded.Distinct();
			var foundAssamblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

			//Read all DLL´s in bin folder...
			string binFolder = Assembly.GetExecutingAssembly().CodeBase;
			binFolder = binFolder.Substring(0, binFolder.LastIndexOf("/")).Substring(8).Replace('/', '\\');

			var directory = new DirectoryInfo(binFolder);
			foreach (var dllFile in directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
			{
				try
				{
					AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile.FullName);
					bool contains = ignoreAssamblieNames.Contains(assemblyName.Name);
					if (!foundAssamblies.Exists(a => a.GetName().Name.Equals(assemblyName.Name, StringComparison.InvariantCultureIgnoreCase))
						&& !contains)
						foundAssamblies.Add(AppDomain.CurrentDomain.Load(assemblyName));
				}
				catch
				{
				}
			}

			var result = foundAssamblies.Where(assembly => !(ignoreAssamblieNames.Any(ia =>
															  ia.Equals(assembly.GetName().Name, StringComparison.InvariantCultureIgnoreCase)
										) || (new[] { "b03f5f7f11d50a3a", "b77a5c561934e089", "31BF3856AD364E35" })
												.Any(p => assembly.GetName().ToString().IndexOf(p, StringComparison.InvariantCultureIgnoreCase) != -1)
									)).Distinct().ToList();

			result.Sort((a, b) =>
				{
					if (ReferenceEquals(a, b))
						return 0;

					var aName = a.GetName().Name;
					var bName = b.GetName().Name;

					var aFirst = firstAssamblieNames.Any(item => item.Equals(aName, StringComparison.InvariantCultureIgnoreCase));
					var bFirst = firstAssamblieNames.Any(item => item.Equals(bName, StringComparison.InvariantCultureIgnoreCase));

					if (aFirst && bFirst)
						return 0;

					if (aFirst)
						return -1;

					if (bFirst)
						return 1;

					var aLast = lastAssemblieNames.Any(item => item.Equals(aName, StringComparison.InvariantCultureIgnoreCase));
					var bLast = lastAssemblieNames.Any(item => item.Equals(bName, StringComparison.InvariantCultureIgnoreCase));

					if (aLast && bLast)
						return 0;

					if (aLast)
						return 1;

					if (bLast)
						return -1;

					return String.Compare(aName, bName);
				});

			return result.ToArray();
		}
	}
}
