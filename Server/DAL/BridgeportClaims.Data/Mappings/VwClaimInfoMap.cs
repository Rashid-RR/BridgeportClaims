using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class VwClaimInfoMap : ClassMap<VwClaimInfo>
    {
        public VwClaimInfoMap()
        {
            Table("vwClaimInfo");
            Schema("dbo");
            ReadOnly();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.RowId).Column("RowID").Not.Nullable();
            Map(x => x.ClaimId).Column("ClaimID").Not.Nullable();
            Map(x => x.ClaimNumber).Column("ClaimNumber").Not.Nullable().Length(255);
            Map(x => x.Name).Column("Name").Length(500);
            Map(x => x.InjuryDate).Column("InjuryDate").Length(30);
            Map(x => x.Carrier).Column("Carrier").Length(255);
        }
    }
}