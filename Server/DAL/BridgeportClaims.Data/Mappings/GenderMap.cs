﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class GenderMap : ClassMap<Gender>
    {
        public GenderMap()
        {
            Table("Gender");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.GenderId).GeneratedBy.Identity().Column("GenderID");
            Map(x => x.GenderName).Column("GenderName").Not.Nullable().Length(55);
            Map(x => x.GenderCode).Column("GenderCode").Not.Nullable().Length(5);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Patient).KeyColumn("GenderID");
        }
    }
}