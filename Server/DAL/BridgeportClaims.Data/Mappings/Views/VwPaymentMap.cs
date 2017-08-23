using BridgeportClaims.Entities.DomainModels.Views;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings.Views
{
    public class VwPaymentMap : ClassMap<VwPayment>
    {
        public VwPaymentMap()
        {
            Table("vwPayment");
            Schema("dbo");
            ReadOnly();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PaymentId).Column("PaymentID").Not.Nullable();
            Map(x => x.CheckNumber).Column("CheckNumber").Not.Nullable().Length(50);
            Map(x => x.AmountPaid).Column("AmountPaid").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.DatePosted).Column("DatePosted");
            Map(x => x.PrescriptionId).Column("PrescriptionID").Precision(10);
            Map(x => x.ClaimId).Column("ClaimID").Precision(10);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}