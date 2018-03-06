using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class FileTypeMap : ClassMap<FileType>
    {
        public FileTypeMap()
        {
            Table("FileType");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.FileTypeId).GeneratedBy.Assigned().Column("FileTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Unique().Length(50);
            Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(10);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Document).KeyColumn("FileTypeID");
        }
    }
}