using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionNoteMap : ClassMap<PrescriptionNote>
    {
        public PrescriptionNoteMap()
        {
            Table("PrescriptionNote");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PrescriptionNoteId).GeneratedBy.Identity().Column("PrescriptionNoteID");
            References(x => x.PrescriptionNoteType).Column("PrescriptionNoteTypeID");
            References(x => x.AspNetUsers).Column("EnteredByUserID");
            Map(x => x.NoteText).Column("NoteText").Not.Nullable().Length(8000);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.PrescriptionNoteMapping).KeyColumn("PrescriptionNoteID");
        }
    }
}