using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Table("Invoice");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.InvoiceId).GeneratedBy.Identity().Column("InvoiceID");
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Not.Nullable().Length(100);
            Map(x => x.InvoiceDate).Column("InvoiceDate").Not.Nullable();
            Map(x => x.Amount).Column("Amount").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.AcctPayable).KeyColumn("InvoiceID");
            HasMany(x => x.Prescription).KeyColumn("InvoiceID");
        }
    }
}
