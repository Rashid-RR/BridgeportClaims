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
            References(x => x.ClaimFlex2).Column("ClaimFlex2ID");
            Map(x => x.PolicyNumber).Column("PolicyNumber").Length(255);
            Map(x => x.DateOfInjury).Column("DateOfInjury");
            Map(x => x.IsFirstParty).Column("IsFirstParty").Not.Nullable();
            Map(x => x.ClaimNumber).Column("ClaimNumber").Not.Nullable().Length(255);
            Map(x => x.PreviousClaimNumber).Column("PreviousClaimNumber").Length(255);
            Map(x => x.PersonCode).Column("PersonCode").Length(2);
            Map(x => x.RelationCode).Column("RelationCode").Precision(3);
            Map(x => x.TermDate).Column("TermDate");
            Map(x => x.UniqueClaimNumber).Column("UniqueClaimNumber").Not.Nullable().Length(258);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.AcctPayable).KeyColumn("ClaimID");
            HasMany(x => x.ClaimNote).KeyColumn("ClaimID");
            HasMany(x => x.ClaimPayment).KeyColumn("ClaimID");
            HasMany(x => x.ClaimsUserHistory).KeyColumn("ClaimID");
            HasMany(x => x.Episode).KeyColumn("ClaimID");
            HasMany(x => x.DocumentIndex).KeyColumn("ClaimID");
            HasMany(x => x.Prescription).KeyColumn("ClaimID");
        }
    }
}