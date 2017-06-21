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
            Map(x => x.FirstName).Column("FirstName").Not.Nullable().Length(255);
            Map(x => x.LastName).Column("LastName").Not.Nullable().Length(255);
            Map(x => x.JoinDate).Column("JoinDate").Not.Nullable();
            Map(x => x.UserName).Column("UserName").Not.Nullable().Unique().Length(256);
            Map(x => x.Email).Column("Email").Unique().Length(256);
            Map(x => x.EmailConfirmed).Column("EmailConfirmed").Not.Nullable();
            Map(x => x.PasswordHash).Column("PasswordHash");
            Map(x => x.SecurityStamp).Column("SecurityStamp");
            Map(x => x.PhoneNumber).Column("PhoneNumber");
            Map(x => x.PhoneNumberConfirmed).Column("PhoneNumberConfirmed").Not.Nullable();
            Map(x => x.TwoFactorEnabled).Column("TwoFactorEnabled").Not.Nullable();
            Map(x => x.LockoutEndDateUtc).Column("LockoutEndDateUtc");
            Map(x => x.LockoutEnabled).Column("LockoutEnabled").Not.Nullable();
            Map(x => x.AccessFailedCount).Column("AccessFailedCount").Not.Nullable().Precision(10);
            HasMany(x => x.AspNetUserClaims).KeyColumn("UserID");
            HasMany(x => x.AspNetUserLogins).KeyColumn("UserID");
            HasMany(x => x.AspNetUserRoles).KeyColumn("UserID");
            HasMany(x => x.ClaimNote).KeyColumn("EnteredByUserID");
            HasMany(x => x.PrescriptionNote).KeyColumn("EnteredByUserID");
        }
    }
}
