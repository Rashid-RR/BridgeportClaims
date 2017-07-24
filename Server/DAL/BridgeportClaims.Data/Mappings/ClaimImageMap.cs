using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimImageMap : ClassMap<ClaimImage>
    {
        public ClaimImageMap()
        {
            Table("ClaimImage");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ClaimImageId).GeneratedBy.Identity().Column("ClaimImageID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.ClaimImageType).Column("ClaimImageTypeID");
            Map(x => x.DateRecorded).Column("DateRecorded");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}