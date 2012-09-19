using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Litium.Common;
using Litium.Common.Configurations;
using Litium.Common.Lifecycle;
using Litium.Infrastructure.DataAccess.Cache;
using Litium.Infrastructure.DataAccess.Collection;
using Litium.Infrastructure.DataAccess.Events;
using Litium.Infrastructure.DataAccess.Events.AutoDirtyCheck;
using Litium.Infrastructure.DataAccess.LinqFunctions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace Litium.Infrastructure.DataAccess.Lifecycle
{
	/// <summary>
	/// 	Setups all necessary NHibernate configurations
	/// </summary>
	public class NHibernateSetup : ISetupTask
	{
		public void Setup(Assembly[] assemblies)
		{
			RegisterEventListeners();
			IoC.Container.For<IAppFabricCacheFactory>().RegisterAsSingleton();
			var sessionFactory = BuildSessionFactory(assemblies);
			IoC.Container.For<ISessionFactory>().RegisterInstance(sessionFactory);
		}

		private static T[] AddPostCommitEventListeners<T>(IEnumerable<T> currentListeners)
		{
			var registeredServices = IoC.ResolveAll<T>()
				.Where(x => x.GetType().GetInterfaces().Contains(typeof(ICommittedEventListener)));

			return  currentListeners == null
					? registeredServices.ToArray()
					: registeredServices.Concat(currentListeners).ToArray();
		}

		private static T[] AddPreCommitEventListeners<T>(IEnumerable<T> currentListeners)
		{
			var registeredServices = IoC.ResolveAll<T>()
				.Where(x => x.GetType().GetInterfaces().Contains(typeof(IEventListener)));

			return currentListeners == null
					? registeredServices.ToArray()
					: registeredServices.Concat(currentListeners).ToArray();
		}

		private static ISessionFactory BuildSessionFactory(IEnumerable<Assembly> assemblies)
		{
            var cfg = new Configuration();
			var config = Fluently.Configure(cfg)
				.Mappings(x =>
				{
					if (assemblies != null)
					{
						x.FluentMappings.Conventions.Add(ForeignKey.EndsWith("Id"),
						                                 DefaultLazy.Always()/*,
						                                 DefaultCascade.All()*/);
						x.FluentMappings.Conventions.AddFromAssemblyOf<NHibernateSetup>();
						foreach (var assembly in assemblies.ToArray())
						{
							x.FluentMappings.AddFromAssembly(assembly);
						}
					}
					x.MergeMappings();
				})
				.Database(GetDatabaseConfiguration())
				.Cache(GetCacheConfiguration())
				.CurrentSessionContext<LazySessionContext>()
				.CollectionTypeFactory<Net4CollectionTypeFactory>()
				.ExposeConfiguration(ConfigureEventListeners())
				.ExposeConfiguration(ConfigureDatabaseSchema())
				.BuildConfiguration()
				.SetProperty(Environment.LinqToHqlGeneratorsRegistry, typeof(LitiumLinqToHqlGeneratorsRegistry).AssemblyQualifiedName)
				.RegisterDisableAutoDirtyCheckListeners();

			return config.BuildSessionFactory();
		}

		private static Action<Configuration> ConfigureDatabaseSchema()
		{
			//For tests
			if (LitiumConfigs.Data.EmptyDb)
			{
				return configuration => new SchemaExport(configuration).Create(false, true);
			}

			return configuration => new SchemaUpdate(configuration).Execute(false, true);
		}

		private static Action<Configuration> ConfigureEventListeners()
		{
			return configuration =>
					{
						configuration.EventListeners.SaveEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.SaveEventListeners);
						configuration.EventListeners.UpdateEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.UpdateEventListeners);
						configuration.EventListeners.SaveOrUpdateEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.SaveOrUpdateEventListeners);
						configuration.EventListeners.MergeEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.MergeEventListeners);
						configuration.EventListeners.DeleteEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.DeleteEventListeners);
						configuration.EventListeners.PreLoadEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PreLoadEventListeners);
						configuration.EventListeners.PreUpdateEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PreUpdateEventListeners);
						configuration.EventListeners.PreInsertEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PreInsertEventListeners);
						configuration.EventListeners.PreDeleteEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PreDeleteEventListeners);
						configuration.EventListeners.PostLoadEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PostLoadEventListeners);
						configuration.EventListeners.PostInsertEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PostInsertEventListeners);
						configuration.EventListeners.PostUpdateEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PostUpdateEventListeners);
						configuration.EventListeners.PostDeleteEventListeners =
							AddPreCommitEventListeners(configuration.EventListeners.PostDeleteEventListeners);
						configuration.EventListeners.PostCommitInsertEventListeners =
							AddPostCommitEventListeners(configuration.EventListeners.PostCommitInsertEventListeners);
						configuration.EventListeners.PostCommitUpdateEventListeners =
							AddPostCommitEventListeners(configuration.EventListeners.PostCommitUpdateEventListeners);
						configuration.EventListeners.PostCommitDeleteEventListeners =
							AddPostCommitEventListeners(configuration.EventListeners.PostCommitDeleteEventListeners);
						configuration.SetListener(ListenerType.Flush, new PostFlushEventListener());
					};
		}

		private static Action<CacheSettingsBuilder> GetCacheConfiguration()
		{
			if (LitiumConfigs.Data.UseCache)
			{
				return cacheSettingsBuilder => cacheSettingsBuilder.UseQueryCache()
												.UseSecondLevelCache()
												.ProviderClass<AppFabricCacheProvider>();
			}
			return cacheSettingsBuilder => { };
		}

		private static Func<IPersistenceConfigurer> GetDatabaseConfiguration()
		{
			return () =>
			       	{
			       		var config = MsSqlConfiguration.MsSql2008;

			       		config = config.ConnectionString(c => c.FromConnectionStringWithKey(LitiumConfigs.Data.ConnectionName));
			       		if (LitiumConfigs.Data.Debug)
			       		{
			       			config = config.ShowSql()
			       				.Raw("generate_statistics", "true");
			       		}

			       		return config;
			       	};
		}

		private static void RegisterEventListeners()
		{
			var listernerTypes = new[]
			                     	{
			                     		typeof (IPreLoadEventListener),
			                     		typeof (IPreUpdateEventListener),
			                     		typeof (IPreInsertEventListener),
			                     		typeof (IPreDeleteEventListener),
			                     		typeof (IPostLoadEventListener),
			                     		typeof (IPostInsertEventListener),
			                     		typeof (IPostUpdateEventListener),
			                     		typeof (IPostDeleteEventListener),
			                     		typeof (ISaveOrUpdateEventListener),
			                     		typeof (IMergeEventListener),
			                     		typeof (IDeleteEventListener)
			                     	};
			foreach (Type listernerType in listernerTypes)
			{
				IoC.Container.For(listernerType).RegisterAsTransient();
			}
		}
	}
}
