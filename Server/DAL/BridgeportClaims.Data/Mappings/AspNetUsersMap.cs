using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class AspNetUsersMap : ClassMap<AspNetUsers>
    {
        public AspNetUsersMap()
        {
            Table("AspNetUsers");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("ID");
            Map(x => x.FirstName).Column("FirstName").Not.Nullable().Length(100);
            Map(x => x.LastName).Column("LastName").Not.Nullable().Length(100);
            Map(x => x.RegisteredDate).Column("RegisteredDate").Not.Nullable();
            Map(x => x.Email).Column("Email").Length(256);
            Map(x => x.EmailConfirmed).Column("EmailConfirmed").Not.Nullable();
            Map(x => x.PasswordHash).Column("PasswordHash").Length(4000);
            Map(x => x.SecurityStamp).Column("SecurityStamp").Length(4000);
            Map(x => x.PhoneNumber).Column("PhoneNumber").Length(30);
            Map(x => x.PhoneNumberConfirmed).Column("PhoneNumberConfirmed").Not.Nullable();
            Map(x => x.TwoFactorEnabled).Column("TwoFactorEnabled").Not.Nullable();
            Map(x => x.LockoutEndDateUtc).Column("LockoutEndDateUtc");
            Map(x => x.LockoutEnabled).Column("LockoutEnabled").Not.Nullable();
            Map(x => x.AccessFailedCount).Column("AccessFailedCount").Not.Nullable().Precision(10);
            Map(x => x.UserName).Column("UserName").Not.Nullable().Length(256);
            HasMany(x => x.AspNetUserClaims).KeyColumn("UserID");
            HasMany(x => x.AspNetUserLogins).KeyColumn("UserID");
            HasMany(x => x.AspNetUserRoles).KeyColumn("UserID");
            HasMany(x => x.ClaimNote).KeyColumn("EnteredByUserID");
            HasMany(x => x.PrescriptionNote).KeyColumn("EnteredByUserID");
        }
    }
}
