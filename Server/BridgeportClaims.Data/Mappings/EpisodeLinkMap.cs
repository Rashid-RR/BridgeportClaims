using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class EpisodeLinkMap : ClassMap<EpisodeLink>
    {
        public EpisodeLinkMap()
        {
            Table("EpisodeLink");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.EpisodeLinkId).GeneratedBy.Identity().Column("EpisodeLinkID");
            References(x => x.EpisodeLinkType).Column("EpisodeLinkTypeID");
            Map(x => x.LinkTransNumber).Column("LinkTransNumber").Length(50);
            Map(x => x.EpisodeNumber).Column("EpisodeNumber").Precision(10);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }
}