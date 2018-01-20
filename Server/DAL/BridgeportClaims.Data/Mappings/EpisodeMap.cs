using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
	public class EpisodeMap : ClassMap<Episode>
	{
		public EpisodeMap()
		{
			Table("Episode");
			Schema("dbo");
			DynamicUpdate();
			SchemaAction.None();
			LazyLoad();
		    Id(x => x.EpisodeId).GeneratedBy.Identity().Column("EpisodeID");
		    References(x => x.Claim).Column("ClaimID");
		    References(x => x.EpisodeType).Column("EpisodeTypeID");
		    References(x => x.ResolvedUserId).Column("ResolvedUserID");
		    References(x => x.AcquiredUserId).Column("AcquiredUserID");
		    References(x => x.AssignedUserId).Column("AssignedUserID");
		    Map(x => x.Note).Column("Note").Not.Nullable().Length(8000);
		    Map(x => x.Role).Column("Role").Length(25);
		    Map(x => x.RxNumber).Column("RxNumber").Length(100);
		    Map(x => x.Status).Column("Status").Length(1);
		    Map(x => x.CreatedDateUtc).Column("CreatedDateUTC");
		    Map(x => x.Description).Column("Description").Length(255);
		    Map(x => x.ResolvedDateUtc).Column("ResolvedDateUTC");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
		    Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
	}
}