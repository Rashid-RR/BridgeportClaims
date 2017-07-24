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
            References(x => x.DiaryType).Column("DiaryTypeID");
            References(x => x.AspNetUsers).Column("EnteredByUserID");
            References(x => x.Claim).Column("ClaimID");
            Map(x => x.NoteText).Column("NoteText").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}