﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Table("Invoice");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.InvoiceId).GeneratedBy.Identity().Column("InvoiceID");
            References(x => x.Payor).Column("PayorID");
            References(x => x.Claim).Column("ClaimID");
            Map(x => x.ARItemKey).Column("ARItemKey").Not.Nullable().Length(255);
            Map(x => x.InvoiceNumber).Column("InvoiceNumber").Not.Nullable().Length(100);
            Map(x => x.InvoiceDate).Column("InvoiceDate").Not.Nullable();
            Map(x => x.Amount).Column("Amount").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            HasMany(x => x.Payment).KeyColumn("InvoiceID");
            HasMany(x => x.Prescription).KeyColumn("InvoiceID");
        }
    }
}
