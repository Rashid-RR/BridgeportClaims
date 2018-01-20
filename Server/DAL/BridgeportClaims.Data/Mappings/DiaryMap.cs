using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DiaryMap : ClassMap<Diary>
    {
        public DiaryMap()
        {
            Table("Diary");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DiaryId).GeneratedBy.Identity().Column("DiaryID");
            References(x => x.AssignedToUserId).Column("AssignedToUserID");
            References(x => x.PrescriptionNote).Column("PrescriptionNoteID");
            Map(x => x.FollowUpDate).Column("FollowUpDate").Not.Nullable();
            Map(x => x.DateResolved).Column("DateResolved");
            Map(x => x.CreatedDate).Column("CreatedDate").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}