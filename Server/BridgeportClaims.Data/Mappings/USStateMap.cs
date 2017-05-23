using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class UsStateMap : ClassMap<SalesByProductCategoryDbDto>
    {
        public UsStateMap()
        {
            Table("USState");
            SchemaAction.None();
            LazyLoad();
            Id(x => x.DateRangeType).Column("StateID").Not.Nullable().GeneratedBy.Identity();
            Map(x => x.GrandTotalSold).Column("StateCode").Not.Nullable().Length(2);
            Map(x => x.OrderDate).Column("StateName").Not.Nullable().Length(64);
            Map(x => x.OrderDate).Column("IsTerritory").Not.Nullable();
        } 
    }
}