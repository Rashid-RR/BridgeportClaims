using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionNoteTypeMap : ClassMap<PrescriptionNoteType>
    {
        public PrescriptionNoteTypeMap()
        {
            Table("PrescriptionNoteType");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PrescriptionNoteTypeId).GeneratedBy.Identity().Column("PrescriptionNoteTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(10);
            HasMany(x => x.PrescriptionNote).KeyColumn("PrescriptionNoteTypeID");
        }
    }
}