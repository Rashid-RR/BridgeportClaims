using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class EpisodeCategoryMap : ClassMap<EpisodeCategory>
    {
        public EpisodeCategoryMap()
        {
            Table("EpisodeCategory");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.EpisodeCategoryId).GeneratedBy.Identity().Column("EpisodeCategoryID");
            Map(x => x.CategoryName).Column("CategoryName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(50);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Episode).KeyColumn("EpisodeCategoryID");
        }
    }
}