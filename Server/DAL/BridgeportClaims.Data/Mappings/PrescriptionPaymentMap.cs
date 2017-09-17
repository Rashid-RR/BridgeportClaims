using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionPaymentMap : ClassMap<PrescriptionPayment>
    {
        public PrescriptionPaymentMap()
        {
            Table("PrescriptionPayment");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PrescriptionPaymentId).GeneratedBy.Identity().Column("PrescriptionPaymentID");
            References(x => x.Prescription).Column("PrescriptionID");
            References(x => x.UserId).Column("UserID");
            Map(x => x.CheckNumber).Column("CheckNumber").Not.Nullable().Length(50);
            Map(x => x.AmountPaid).Column("AmountPaid").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.DatePosted).Column("DatePosted");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}