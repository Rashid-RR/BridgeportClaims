using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimFlex2Map : ClassMap<ClaimFlex2>
    {
        public ClaimFlex2Map()
        {
            Table("ClaimFlex2");
            LazyLoad();
            Id(x => x.ClaimFlex2Id).GeneratedBy.Identity().Column("ClaimFlex2ID");
            Map(x => x.Flex2).Column("Flex2").Not.Nullable().Unique().Length(10);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Claim).KeyColumn("ClaimFlex2ID");
        }
    }
}