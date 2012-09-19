using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public sealed class PageTypeMap : ClassMap<PageType>
	{
		public PageTypeMap()
		{
			Id(x => x.Id);

			Map(x => x.Name)
				.Not.Nullable();
			Component(x => x.Behaviour, m =>
			                            	{
			                            		m.Map(x => x.CanBeInMenu);
			                            		m.Map(x => x.CanBeInSiteMap);
			                            		m.Map(x => x.IsInAnalytics);
												//m.Map(x => x.CanBeLinkedTo);
			                            		m.Map(x => x.CanBeMasterPage);
			                            		m.Map(x => x.CanBeDeleted);
												//m.Map(x => x.CanBePrinted);
			                            		m.Map(x => x.IsSearchable);
												//m.Map(x => x.CanBeVersioned);
			                            		m.Map(x => x.EditableInUI);
												//m.Map(x => x.PageVersionsToKeep);
			                            	});
			Map(x => x.PageTypeCategory);
			//Map(x => x.PossibleChildPageTypes)
			//    .Column("ChildPageTypeNames")
			//    .CustomType<ListToStringType>();
			//Map(x => x.PossibleParentPageTypes)
			//    .Column("ParentPageTypeNames")
			//    .CustomType<ListToStringType>();

			//HasManyToMany(x => x.WebSites)
			//    .Schema("dbo")
			//    .Table("MediaArchive_Category_File")
			//    .Cascade.SaveUpdate()
			//    .LazyLoad();
		}
	}
}