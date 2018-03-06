using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class InvoiceIndexMap : ClassMap<InvoiceIndex>
    {
        public InvoiceIndexMap()
        {
            Table("InvoiceIndex");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DocumentId).GeneratedBy.Identity().Column("DocumentID");
            References(x => x.Document).Column("DocumentID");
            References(x => x.ModifiedByUser).Column("ModifiedByUserID");
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Not.Nullable().Length(100);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}