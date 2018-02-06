using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class EpisodeNoteMap : ClassMap<EpisodeNote>
    {
        public EpisodeNoteMap()
        {
            Table("EpisodeNote");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.EpisodeNoteId).GeneratedBy.Identity().Column("EpisodeNoteID");
            References(x => x.Episode).Column("EpisodeID");
            References(x => x.WrittenByUser).Column("WrittenByUserID");
            Map(x => x.NoteText).Column("NoteText").Not.Nullable().Length(8000);
            Map(x => x.Created).Column("Created").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}