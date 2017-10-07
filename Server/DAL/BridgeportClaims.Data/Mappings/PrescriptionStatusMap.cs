using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriptionStatusMap : ClassMap<PrescriptionStatus>
    {
        public PrescriptionStatusMap()
        {
            Table("PrescriptionStatus");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.PrescriptionStatusId).GeneratedBy.Identity().Column("PrescriptionStatusID");
            Map(x => x.StatusName).Column("StatusName").Not.Nullable().Unique().Length(100);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Prescription).KeyColumn("PrescriptionStatusID");
        }
    }
}