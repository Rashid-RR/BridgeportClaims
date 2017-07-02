using BridgeportClaims.Entities.DomainModels.Views;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings.Views
{
    public class VwPrescriptionNoteMap : ClassMap<VwPrescriptionNote>
    {
        public VwPrescriptionNoteMap()
        {
            Table("vwPrescriptionNote WITH (NOEXPAND)");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.PrescriptionId, "PrescriptionID")
                .KeyProperty(x => x.PrescriptionNoteId, "PrescriptionNoteID");
            Map(x => x.RxNumber).Column("RxNumber").Not.Nullable().Length(100);
            Map(x => x.DateFilled).Column("DateFilled").Not.Nullable();
            Map(x => x.LabelName).Column("LabelName").Length(25);
            Map(x => x.PrescriptionNoteType).Column("PrescriptionNoteType").Not.Nullable().Length(255);
            Map(x => x.NoteText).Column("NoteText").Not.Nullable().Length(8000);
            Map(x => x.NoteAuthor).Column("NoteAuthor").Not.Nullable().Length(201);
            Map(x => x.NoteCreatedOn).Column("NoteCreatedOn").Not.Nullable();
            Map(x => x.NoteUpdatedOn).Column("NoteUpdatedOn").Not.Nullable();
        }
    }
}