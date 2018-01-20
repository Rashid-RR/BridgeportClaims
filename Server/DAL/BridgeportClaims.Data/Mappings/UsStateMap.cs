﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class UsStateMap : ClassMap<UsState>
    {
        public UsStateMap()
        {
            Table("UsState");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.StateId).GeneratedBy.Identity().Column("StateID");
            Map(x => x.StateCode).Column("StateCode").Not.Nullable().Unique().Length(2);
            Map(x => x.StateName).Column("StateName").Not.Nullable().Length(64);
            Map(x => x.IsTerritory).Column("IsTerritory").Not.Nullable();
            HasMany(x => x.Claim).KeyColumn("JurisdictionStateID");
            HasMany(x => x.Patient).KeyColumn("StateID");
            HasMany(x => x.Payor).KeyColumn("BillToStateID");
            HasMany(x => x.Pharmacy).KeyColumn("StateID");
            HasMany(x => x.Prescriber).KeyColumn("StateID");
        } 
    }
}