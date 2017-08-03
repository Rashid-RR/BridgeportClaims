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
			References(x => x.AspNetUsers).Column("AssignedUserID");
			Map(x => x.Note).Column("Note").Length(1000);
			Map(x => x.Role).Column("Role").Length(10);
			Map(x => x.Type).Column("Type").Length(50);
			Map(x => x.ResolvedUser).Column("ResolvedUser").Length(100);
			Map(x => x.AcquiredUser).Column("AcquiredUser").Length(100);
			Map(x => x.RxNumber).Column("RxNumber").Length(100);
			Map(x => x.Status).Column("Status").Length(1);
			Map(x => x.CreatedDate).Column("CreatedDate");
			Map(x => x.Description).Column("Description").Length(255);
			Map(x => x.ResolvedDate).Column("ResolvedDate");
			Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
			Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
		}
	}
}