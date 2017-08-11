using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ImportFileMap : ClassMap<ImportFile>
    {
        public ImportFileMap()
        {
            Table("ImportFile");
            Schema("util");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ImportFileId).GeneratedBy.Identity().Column("ImportFileID");
            Map(x => x.FileBytes).Column("FileBytes").Not.Nullable();
            Map(x => x.FileName).Column("FileName").Not.Nullable().Length(255);
            Map(x => x.FileExtension).Column("FileExtension").Length(30);
            Map(x => x.FileSize).Column("FileSize").Not.Nullable().Length(100);
            Map(x => x.Processed).Column("Processed").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}
