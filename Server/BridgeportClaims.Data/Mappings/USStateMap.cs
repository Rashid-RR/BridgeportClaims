using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class UsStateMap : ClassMap<DbccUserOptionsResults>
    {
        public UsStateMap()
        {
            Table("USState");
            SchemaAction.None();
            LazyLoad();
            Id(x => x.SetOption).Column("StateID").Not.Nullable().GeneratedBy.Identity();
            Map(x => x.Value).Column("StateCode").Not.Nullable().Length(2);
            Map(x => x.SetOption).Column("StateName").Not.Nullable().Length(64);
            Map(x => x.Value).Column("IsTerritory").Not.Nullable();
        } 
    }
}