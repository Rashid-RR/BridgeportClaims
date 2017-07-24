using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimNoteMap : ClassMap<ClaimNote>
    {
        public ClaimNoteMap()
        {
            Table("ClaimNote");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ClaimId).GeneratedBy.Identity().Column("ClaimID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.ClaimNoteType).Column("ClaimNoteTypeID");
            References(x => x.AspNetUsers).Column("EnteredByUserID");
            Map(x => x.NoteText).Column("NoteText").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}
