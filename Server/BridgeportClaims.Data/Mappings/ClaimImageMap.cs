using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimImageMap : ClassMap<ClaimImage>
    {
        public ClaimImageMap()
        {
            Table("ClaimImage");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ClaimImageId).GeneratedBy.Identity().Column("ClaimImageID");
            References(x => x.Claim).Column("ClaimID");
            Map(x => x.ImageNumber).Column("ImageNumber").Precision(10);
            Map(x => x.ImageType).Column("ImageType").Length(255);
            Map(x => x.Daterec).Column("Daterec");
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }
}