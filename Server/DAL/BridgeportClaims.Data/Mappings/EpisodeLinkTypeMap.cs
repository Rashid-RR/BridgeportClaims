﻿using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class EpisodeLinkTypeMap : ClassMap<EpisodeLinkType>
    {
        public EpisodeLinkTypeMap()
        {
            Table("EpisodeLinkType");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.EpisodeLinkTypeId).GeneratedBy.Identity().Column("EpisodeLinkTypeID");
            Map(x => x.EpisodeLinkName).Column("EpisodeLinkName").Not.Nullable().Length(50);
            Map(x => x.EpisodeLinkCode).Column("EpisodeLinkCode").Not.Nullable().Length(10);
            Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
            Map(x => x.UpdatedOn).Column("UpdatedOn").Not.Nullable();
            HasMany(x => x.EpisodeLink).KeyColumn("EpisodeLinkTypeID");
        }
    }
}