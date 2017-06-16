using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PaymentMap : ClassMap<Payment>
    {
        public PaymentMap()
        {
            Table("Payment");
            Schema("dbo");
            DynamicUpdate();
            LazyLoad();
            SchemaAction.None();
            Id(x => x.Id).GeneratedBy.Identity().Column("PaymentID");
            References(x => x.Claim).Column("ClaimId");
            Map(x => x.CheckNumber).Column("CheckNumber").Not.Nullable().Length(100);
            Map(x => x.CheckDate).Column("CheckDate").Not.Nullable();
            Map(x => x.AmountPaid).Column("AmountPaid").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Not.Nullable().Length(100);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }
}