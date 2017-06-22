using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AspNetUserRolesMap : ClassMap<AspNetUserRoles>
    {
        public AspNetUserRolesMap()
        {
            Table("AspNetUserRoles");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.UserId, "UserID")
                .KeyProperty(x => x.RoleId, "RoleID");
            References(x => x.AspNetUsers).Column("UserID");
            References(x => x.AspNetRoles).Column("RoleID");
        }
    }
}