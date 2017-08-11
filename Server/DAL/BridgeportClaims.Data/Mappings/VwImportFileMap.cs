using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class VwImportFileMap : ClassMap<VwImportFile>
    {
        public VwImportFileMap()
        {
            Table("vwImportFile");
            Schema("util");
            ReadOnly();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ImportFileId).Column("ImportFileID").Not.Nullable();
            Map(x => x.FileName).Column("FileName").Not.Nullable().Length(255);
            Map(x => x.FileExtension).Column("FileExtension").Length(30);
            Map(x => x.FileSize).Column("FileSize").Not.Nullable().Length(100);
            Map(x => x.FileType).Column("FileType").Not.Nullable().Length(255);
            Map(x => x.Processed).Column("Processed").Not.Nullable().Length(5);
            Map(x => x.CreatedOnLocal).Column("CreatedOnLocal");
            Map(x => x.UpdatedOnLocal).Column("UpdatedOnLocal");
        }
    }
}
