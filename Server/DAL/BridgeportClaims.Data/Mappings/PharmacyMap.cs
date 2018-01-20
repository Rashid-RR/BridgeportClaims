using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PharmacyMap : ClassMap<Pharmacy>
    {
        public PharmacyMap()
        {
            Table("Pharmacy");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.Nabp).GeneratedBy.Assigned().Column("NABP");
            References(x => x.StateId).Column("StateID");
            Map(x => x.Npi).Column("NPI").Length(10);
            Map(x => x.PharmacyName).Column("PharmacyName").Length(60);
            Map(x => x.Address1).Column("Address1").Length(55);
            Map(x => x.Address2).Column("Address2").Length(55);
            Map(x => x.City).Column("City").Length(35);
            Map(x => x.PostalCode).Column("PostalCode").Length(11);
            Map(x => x.PhoneNumber).Column("PhoneNumber").Length(30);
            Map(x => x.AlternatePhoneNumber).Column("AlternatePhoneNumber").Length(30);
            Map(x => x.FaxNumber).Column("FaxNumber").Length(30);
            Map(x => x.Contact).Column("Contact").Length(55);
            Map(x => x.ContactPhoneNumber).Column("ContactPhoneNumber").Length(30);
            Map(x => x.ContactEmailAddress).Column("ContactEmailAddress").Length(50);
            Map(x => x.FederalTin).Column("FederalTIN").Length(15);
            Map(x => x.DispType).Column("DispType").Length(2);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Prescription).KeyColumn("PharmacyNABP");
        }
    }
}