using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionMap : ClassMap<Prescription>
    {
        public PrescriptionMap()
        {
            Table("Prescription");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.PrescriptionId).GeneratedBy.Identity().Column("PrescriptionID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.PharmacyNabp).Column("PharmacyNABP");
            References(x => x.Invoice).Column("InvoiceID");
            References(x => x.PrescriptionStatus).Column("PrescriptionStatusID");
            Map(x => x.RxNumber).Column("RxNumber").Not.Nullable().Length(100);
            Map(x => x.DateSubmitted).Column("DateSubmitted").Not.Nullable();
            Map(x => x.DateFilled).Column("DateFilled").Not.Nullable();
            Map(x => x.LabelName).Column("LabelName").Length(25);
            Map(x => x.Ndc).Column("NDC").Not.Nullable().Length(11);
            Map(x => x.Quantity).Column("Quantity").Not.Nullable().Precision(53);
            Map(x => x.DaySupply).Column("DaySupply").Not.Nullable().Precision(53);
            Map(x => x.Generic).Column("Generic").Not.Nullable().Length(1);
            Map(x => x.AwpUnit).Column("AWPUnit").Precision(53);
            Map(x => x.Usual).Column("Usual").Precision(18);
            Map(x => x.Prescriber).Column("Prescriber").Length(100);
            Map(x => x.PayableAmount).Column("PayableAmount").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.BilledAmount).Column("BilledAmount").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.TransactionType).Column("TransactionType").Not.Nullable().Length(1);
            Map(x => x.Compound).Column("Compound").Not.Nullable().Length(1);
            Map(x => x.TranId).Column("TranID").Not.Nullable().Length(14);
            Map(x => x.RefillDate).Column("RefillDate");
            Map(x => x.RefillNumber).Column("RefillNumber").Precision(5);
            Map(x => x.Mony).Column("MONY").Length(1);
            Map(x => x.Daw).Column("DAW").Precision(5);
            Map(x => x.Gpi).Column("GPI").Length(14);
            Map(x => x.BillIngrCost).Column("BillIngrCost").Precision(53);
            Map(x => x.BillDispFee).Column("BillDispFee").Precision(53);
            Map(x => x.BilledTax).Column("BilledTax").Precision(53);
            Map(x => x.BilledCopay).Column("BilledCopay").Precision(53);
            Map(x => x.PayIngrCost).Column("PayIngrCost").Precision(53);
            Map(x => x.PayDispFee).Column("PayDispFee").Precision(53);
            Map(x => x.PayTax).Column("PayTax").Precision(53);
            Map(x => x.Dea).Column("DEA").Length(12);
            Map(x => x.PrescriberNpi).Column("PrescriberNPI").Length(12);
            Map(x => x.Strength).Column("Strength").Length(255);
            Map(x => x.GpiGenName).Column("GPIGenName").Length(255);
            Map(x => x.TheraClass).Column("TheraClass").Length(255);
            Map(x => x.Awp).Column("AWP").Precision(53);
            Map(x => x.ReversedDate).Column("ReversedDate");
            Map(x => x.IsReversed).Column("IsReversed");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.PrescriptionNoteMapping).KeyColumn("PrescriptionID");
            HasMany(x => x.PrescriptionPayment).KeyColumn("PrescriptionID");
        }
    }
}