using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimMap : ClassMap<Claim>
    {
        public ClaimMap()
        {
            Table("Claim");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.ClaimId).GeneratedBy.Identity().Column("ClaimID");
            References(x => x.Payor).Column("PayorID");
            References(x => x.Adjustor).Column("AdjusterID");
            References(x => x.UsState).Column("JurisdictionStateID");
            References(x => x.Patient).Column("PatientID");
            Map(x => x.PolicyNumber).Column("PolicyNumber").Length(255);
            Map(x => x.DateOfInjury).Column("DateOfInjury");
            Map(x => x.IsFirstParty).Column("IsFirstParty").Not.Nullable();
            Map(x => x.ClaimNumber).Column("ClaimNumber").Not.Nullable().Length(255);
            Map(x => x.PreviousClaimNumber).Column("PreviousClaimNumber").Length(255);
            Map(x => x.PersonCode).Column("PersonCode").Precision(10);
            Map(x => x.RelationCode).Column("RelationCode").Precision(3);
            Map(x => x.TermDate).Column("TermDate");
            Map(x => x.UniqueClaimNumber).Column("UniqueClaimNumber").Not.Nullable().Length(258);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            HasMany(x => x.ClaimImage).KeyColumn("ClaimID");
            HasOne(x => x.ClaimNote);

            /*HasOn             (x => x.ClaimNote).KeyColumn("ClaimID");
            HasMany(x => x.Episode).KeyColumn("ClaimID");
            HasMany(x => x.Invoice).KeyColumn("ClaimID");
            HasMany(x => x.Payment).KeyColumn("ClaimID");
            HasMany(x => x.Prescription).KeyColumn("ClaimID");*/
        }
    }
}