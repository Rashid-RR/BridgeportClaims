using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DocumentTypeMap : ClassMap<DocumentType>
    {
        public DocumentTypeMap()
        {
            Table("DocumentType");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DocumentTypeId).GeneratedBy.Assigned().Column("DocumentTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(10);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.DocumentIndex).KeyColumn("DocumentTypeID");
        }
    }
}