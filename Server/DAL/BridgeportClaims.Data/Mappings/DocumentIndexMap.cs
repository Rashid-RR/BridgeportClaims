using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class DocumentIndexMap : ClassMap<DocumentIndex>
    {
        public DocumentIndexMap()
        {
            Table("DocumentIndex");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DocumentId).GeneratedBy.Identity().Column("DocumentID");
            References(x => x.Document).Column("DocumentID");
            References(x => x.Claim).Column("ClaimID");
            References(x => x.DocumentType).Column("DocumentTypeID");
            References(x => x.AspNetUsers).Column("IndexedByUserID");
            Map(x => x.RxDate).Column("RxDate");
            Map(x => x.RxNumber).Column("RxNumber").Length(100);
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Length(100);
            Map(x => x.InjuryDate).Column("InjuryDate");
            Map(x => x.AttorneyName).Column("AttorneyName").Length(255);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}