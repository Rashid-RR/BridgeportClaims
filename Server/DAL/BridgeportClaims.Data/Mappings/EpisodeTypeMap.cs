using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
	public class EpisodeTypeMap : ClassMap<EpisodeType>
	{
		public EpisodeTypeMap()
		{
			Table("EpisodeType");
			Schema("dbo");
			DynamicUpdate();
			SchemaAction.None();
			LazyLoad();
		    Id(x => x.EpisodeTypeId).GeneratedBy.Assigned().Column("EpisodeTypeID");
		    Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
		    Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(10);
		    Map(x => x.Description).Column("Description").Length(1000);
		    Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
		    Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
		    HasMany(x => x.DocumentTypeEpisodeTypeMapping).KeyColumn("EpisodeTypeID");
		    HasMany(x => x.Episode).KeyColumn("EpisodeTypeID");
        }
	}
}