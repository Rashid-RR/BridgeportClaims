using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class UsStateMap : ClassMap<UsState>
    {
        public UsStateMap()
        {
            Table("UsState");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("StateID");
            Map(x => x.StateCode).Column("StateCode").Not.Nullable().Length(2);
            Map(x => x.StateName).Column("StateName").Not.Nullable().Length(64);
            Map(x => x.IsTerritory).Column("IsTerritory").Not.Nullable();
            HasMany(x => x.Claim).KeyColumn("JurisdictionStateID");
            HasMany(x => x.Payor).KeyColumn("BillToStateID");
        } 
    }
}