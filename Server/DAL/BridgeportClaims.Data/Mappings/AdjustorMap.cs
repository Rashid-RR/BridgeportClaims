﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AdjustorMap : ClassMap<Adjustor>
    {
        public AdjustorMap()
        {
            Table("Adjustor");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.AdjustorId).GeneratedBy.Identity().Column("AdjustorID");
            References(x => x.Payor).Column("PayorID");
            References(x => x.ModifiedByUserId).Column("ModifiedByUserID");
            Map(x => x.AdjustorName).Column("AdjustorName").Not.Nullable().Length(255);
            Map(x => x.PhoneNumber).Column("PhoneNumber").Length(30);
            Map(x => x.FaxNumber).Column("FaxNumber").Length(30);
            Map(x => x.EmailAddress).Column("EmailAddress").Length(155);
            Map(x => x.Extension).Column("Extension").Length(10);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Claim).KeyColumn("AdjusterID");
        }
    }
}