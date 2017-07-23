using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimsUserHistoryMap : ClassMap<ClaimsUserHistory>
    {
        public ClaimsUserHistoryMap()
        {
            Table("ClaimsUserHistory");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ClaimsUserHistoryId).GeneratedBy.Identity().Column("ClaimsUserHistoryID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.AspNetUsers).Column("UserID");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}