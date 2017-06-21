using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PatientMap : ClassMap<Patient>
    {

        public PatientMap()
        {
            Table("Patient");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PatientId).GeneratedBy.Identity().Column("PatientID");
            References(x => x.UsState).Column("StateID");
            References(x => x.Gender).Column("GenderID");
            Map(x => x.LastName).Column("LastName").Not.Nullable().Length(155);
            Map(x => x.FirstName).Column("FirstName").Not.Nullable().Length(155);
            Map(x => x.Address1).Column("Address1").Length(255);
            Map(x => x.Address2).Column("Address2").Length(255);
            Map(x => x.City).Column("City").Length(155);
            Map(x => x.PostalCode).Column("PostalCode").Length(100);
            Map(x => x.PhoneNumber).Column("PhoneNumber").Length(30);
            Map(x => x.AlternatePhoneNumber).Column("AlternatePhoneNumber").Length(30);
            Map(x => x.EmailAddress).Column("EmailAddress").Length(155);
            Map(x => x.DateOfBirth).Column("DateOfBirth");
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            HasMany(x => x.Claim).KeyColumn("PatientID");
        }
    }
}