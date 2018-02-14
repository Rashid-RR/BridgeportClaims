using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class EpisodeTypeUsersMappingMap : ClassMap<EpisodeTypeUsersMapping>
    {
        public EpisodeTypeUsersMappingMap()
        {
            Table("EpisodeTypeUsersMapping");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.EpisodeTypeId, "EpisodeTypeID")
                .KeyProperty(x => x.UserId, "UserID");
            References(x => x.EpisodeType).Column("EpisodeTypeID");
            References(x => x.User).Column("UserID");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}