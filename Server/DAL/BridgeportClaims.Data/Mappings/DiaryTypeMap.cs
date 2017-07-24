using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DiaryTypeMap : ClassMap<DiaryType>
    {
        public DiaryTypeMap()
        {
            Table("DiaryType");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.DiaryTypeId).GeneratedBy.Identity().Column("DiaryTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(10);
            Map(x => x.Description).Column("Description").Length(1000);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Diary).KeyColumn("DiaryTypeID");
        }
    }
}