using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Table("Image");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.ImageId).GeneratedBy.Identity().Column("ImageID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.ImageType).Column("ImageTypeID");
            Map(x => x.CreatedDateLocal).Column("CreatedDateLocal").Not.Nullable();
            Map(x => x.IsIndexed).Column("IsIndexed");
            Map(x => x.RxDate).Column("RxDate");
            Map(x => x.RxNumber).Column("RxNumber").Length(100);
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Length(100);
            Map(x => x.InjuryDate).Column("InjuryDate");
            Map(x => x.AttorneyName).Column("AttorneyName").Length(255);
            Map(x => x.FileName).Column("FileName").Not.Nullable().Length(1000);
            Map(x => x.FileUrl).Column("FileUrl").Length(4000);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}