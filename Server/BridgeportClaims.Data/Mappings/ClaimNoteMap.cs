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
            Id(x => x.ClaimNoteId).GeneratedBy.Identity().Column("ClaimNoteID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.ClaimNoteType).Column("ClaimNoteTypeID");
            References(x => x.AspNetUsers).Column("EnteredByUserID");
            Map(x => x.NoteText).Column("NoteText").Length(8000);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }
}
