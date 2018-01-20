using FluentNHibernate.Mapping;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.Mappings
{
    public class PayorMap : ClassMap<Payor>
    {
        public PayorMap()
        {
            Table("Payor");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.PayorId).GeneratedBy.Identity().Column("PayorID");
            References(x => x.BillToStateId).Column("BillToStateID");
            Map(x => x.GroupName).Column("GroupName").Not.Nullable().Unique().Length(255);
            Map(x => x.BillToName).Column("BillToName").Not.Nullable().Length(255);
            Map(x => x.BillToAddress1).Column("BillToAddress1").Length(255);
            Map(x => x.BillToAddress2).Column("BillToAddress2").Length(255);
            Map(x => x.BillToCity).Column("BillToCity").Length(155);
            Map(x => x.BillToPostalCode).Column("BillToPostalCode").Length(100);
            Map(x => x.PhoneNumber).Column("PhoneNumber").Length(30);
            Map(x => x.AlternatePhoneNumber).Column("AlternatePhoneNumber").Length(30);
            Map(x => x.FaxNumber).Column("FaxNumber").Length(30);
            Map(x => x.Notes).Column("Notes").Length(8000);
            Map(x => x.Contact).Column("Contact").Length(255);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Adjustor).KeyColumn("PayorID");
            HasMany(x => x.Claim).KeyColumn("PayorID");
        }
    }
}
