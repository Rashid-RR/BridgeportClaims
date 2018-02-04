using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DocumentTypeEpisodeTypeMappingMap : ClassMap<DocumentTypeEpisodeTypeMapping>
    {
        public DocumentTypeEpisodeTypeMappingMap()
        {
            Table("DocumentTypeEpisodeTypeMappingMap");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.DocumentTypeId, "DocumentTypeID")
                .KeyProperty(x => x.EpisodeTypeId, "EpisodeTypeID");
            References(x => x.DocumentType).Column("DocumentTypeID");
            References(x => x.EpisodeType).Column("EpisodeTypeID");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            Map(x => x.DataVersion).Column("DataVersion").Not.Nullable();
        }
    }
}
