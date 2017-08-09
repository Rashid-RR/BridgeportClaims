using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AcctPayableMap : ClassMap<AcctPayable>
    {
        public AcctPayableMap()
        {
            Table("AcctPayable");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.AcctPayableId).GeneratedBy.Identity().Column("AcctPayableID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.Invoice).Column("InvoiceID");
            Map(x => x.CheckNumber).Column("CheckNumber").Not.Nullable().Length(100);
            Map(x => x.CheckDate).Column("CheckDate").Not.Nullable();
            Map(x => x.AmountPaid).Column("AmountPaid").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}
