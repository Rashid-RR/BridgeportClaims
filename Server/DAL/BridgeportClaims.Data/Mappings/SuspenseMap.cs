using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class SuspenseMap : ClassMap<Suspense>
    {
        public SuspenseMap()
        {
            Table("Suspense");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.SuspenseId).GeneratedBy.Identity().Column("SuspenseID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.UserId).Column("UserID");
            Map(x => x.CheckNumber).Column("CheckNumber").Not.Nullable().Length(50);
            Map(x => x.AmountRemaining).Column("AmountRemaining").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.SuspenseDate).Column("SuspenseDate").Not.Nullable();
            Map(x => x.NoteText).Column("NoteText").Length(255);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}