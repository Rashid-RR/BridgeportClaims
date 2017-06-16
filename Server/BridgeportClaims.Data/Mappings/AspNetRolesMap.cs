using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AspNetRolesMap : ClassMap<AspNetRoles>
    {
        public AspNetRolesMap()
        {
            Table("AspNetRoles");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("ID");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(256);
            HasMany(x => x.AspNetUserRoles).KeyColumn("RoleID");
        }
    }
}