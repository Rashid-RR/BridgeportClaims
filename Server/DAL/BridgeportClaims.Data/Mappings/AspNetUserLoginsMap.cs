
using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AspNetUserLoginsMap : ClassMap<AspNetUserLogins>
    {
        public AspNetUserLoginsMap()
        {
            Table("AspNetUserLogins");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            CompositeId().KeyProperty(x => x.LoginProvider, "LoginProvider")
                .KeyProperty(x => x.ProviderKey, "ProviderKey")
                .KeyProperty(x => x.UserId, "UserID");
            References(x => x.AspNetUsers).Column("UserID");
        }
    }
}