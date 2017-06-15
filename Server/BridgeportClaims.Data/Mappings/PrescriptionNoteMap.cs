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
            Id(x => x.Id).GeneratedBy.Identity().Column("PrescriptionNoteID");
            References(x => x.PrescriptionNoteType).Column("PrescriptionNoteTypeID");
            References(x => x.AspNetUsers).Column("EnteredByUserID");
            Map(x => x.NoteText).Column("NoteText").Not.Nullable().Length(8000);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            Map(x => x.DataVersion).Column("DataVersion").Not.Nullable();
        }
    }
}