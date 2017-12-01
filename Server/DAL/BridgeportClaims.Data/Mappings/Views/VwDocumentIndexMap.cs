using BridgeportClaims.Entities.DomainModels.Views;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings.Views
{
    public class VwDocumentIndexMap : ClassMap<VwDocumentIndex>
    {
        public VwDocumentIndexMap()
        {
            Table("vwDocumentIndex");
            Schema("dbo");
            ReadOnly();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DocumentId).Column("DocumentID").Not.Nullable();
            Map(x => x.FileName).Column("FileName").Not.Nullable().Length(1000);
            Map(x => x.Extension).Column("Extension").Not.Nullable().Length(50);
            Map(x => x.FileSize).Column("FileSize").Not.Nullable().Length(50);
            Map(x => x.CreationTimeLocal).Column("CreationTimeLocal").Not.Nullable();
            Map(x => x.LastAccessTimeLocal).Column("LastAccessTimeLocal").Not.Nullable();
            Map(x => x.LastWriteTimeLocal).Column("LastWriteTimeLocal").Not.Nullable();
            Map(x => x.DirectoryName).Column("DirectoryName").Length(255);
            Map(x => x.FullFilePath).Column("FullFilePath").Not.Nullable().Length(4000);
            Map(x => x.FileUrl).Column("FileUrl").Length(4000);
            Map(x => x.IsIndexed).Column("IsIndexed");
            Map(x => x.ClaimId).Column("ClaimID").Precision(10);
            Map(x => x.DocumentTypeId).Column("DocumentTypeID").Precision(3);
            Map(x => x.RxDate).Column("RxDate");
            Map(x => x.RxNumber).Column("RxNumber").Length(100);
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Length(100);
            Map(x => x.InjuryDate).Column("InjuryDate");
            Map(x => x.AttorneyName).Column("AttorneyName").Length(255);
        }
    }
}