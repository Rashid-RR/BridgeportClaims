﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class ClaimNoteTypeMap : ClassMap<ClaimNoteType>
    {
        public ClaimNoteTypeMap()
        {
            Table("ClaimNoteType");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.ClaimNoteTypeId).GeneratedBy.Identity().Column("ClaimNoteTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(10);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            HasMany(x => x.ClaimNote).KeyColumn("ClaimNoteTypeID");
        }
    }
}