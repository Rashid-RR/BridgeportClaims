using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionNoteMappingMap : ClassMap<PrescriptionNoteMapping>
    {
        public PrescriptionNoteMappingMap()
        {
            Table("PrescriptionNoteMapping");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.PrescriptionId, "PrescriptionID")
                .KeyProperty(x => x.PrescriptionNoteId, "PrescriptionNoteID");
            References(x => x.Prescription).Column("PrescriptionID");
            References(x => x.PrescriptionNote).Column("PrescriptionNoteID");
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }
}