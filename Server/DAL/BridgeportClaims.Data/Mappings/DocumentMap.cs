using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Table("Document");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DocumentId).GeneratedBy.Identity().Column("DocumentID");
            References(x => x.ModifiedByUserId).Column("ModifiedByUserID");
            Map(x => x.FileName).Column("FileName").Not.Nullable().Unique().Length(1000);
            Map(x => x.Extension).Column("Extension").Not.Nullable().Length(50);
            Map(x => x.FileSize).Column("FileSize").Not.Nullable().Length(50);
            Map(x => x.CreationTimeLocal).Column("CreationTimeLocal").Not.Nullable();
            Map(x => x.LastAccessTimeLocal).Column("LastAccessTimeLocal").Not.Nullable();
            Map(x => x.LastWriteTimeLocal).Column("LastWriteTimeLocal").Not.Nullable();
            Map(x => x.DirectoryName).Column("DirectoryName").Not.Nullable().Length(255);
            Map(x => x.FullFilePath).Column("FullFilePath").Not.Nullable().Length(4000);
            Map(x => x.FileUrl).Column("FileUrl").Not.Nullable().Unique().Length(500);
            Map(x => x.DocumentDate).Column("DocumentDate");
            Map(x => x.ByteCount).Column("ByteCount").Not.Nullable().Precision(19);
            Map(x => x.Archived).Column("Archived").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.DocumentIndex).KeyColumn("DocumentID");
        }
    }
}