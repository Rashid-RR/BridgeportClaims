using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ImportFileTypeMap : ClassMap<ImportFileType>
    {
        public ImportFileTypeMap()
        {
            Table("ImportFileType");
            Schema("util");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ImportFileTypeId).GeneratedBy.Identity().Column("ImportFileTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(30);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.ImportFile).KeyColumn("ImportFileTypeID");
        }
    }
}
