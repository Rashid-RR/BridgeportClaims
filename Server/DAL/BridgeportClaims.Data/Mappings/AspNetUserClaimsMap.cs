using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AspNetUserClaimsMap : ClassMap<AspNetUserClaims>
    {
        public AspNetUserClaimsMap()
        {
            Table("AspNetUserClaims");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.AspNetUsers).Column("UserID");
            Map(x => x.ClaimType).Column("ClaimType").Length(4000);
            Map(x => x.ClaimValue).Column("ClaimValue").Length(4000);
        }
    }
}