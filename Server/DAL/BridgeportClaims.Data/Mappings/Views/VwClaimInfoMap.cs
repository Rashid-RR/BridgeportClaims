using BridgeportClaims.Entities.DomainModels.Views;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings.Views
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
            Id(x => x.ClaimId).Column("ClaimID").Not.Nullable().Precision(10);
            Map(x => x.ClaimNumber).Column("ClaimNumber").Not.Nullable().Length(255);
            Map(x => x.Name).Column("Name").Length(311);
            Map(x => x.Carrier).Column("Carrier").Not.Nullable().Length(255);
            Map(x => x.InjuryDate).Column("InjuryDate");
        }
    }
}