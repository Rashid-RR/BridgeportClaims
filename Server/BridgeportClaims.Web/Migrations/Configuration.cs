using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "jordan.gurney@inContact.com",
                Email = "jordan.gurney@inContact.com",
                EmailConfirmed = true,
                FirstName = "Jordan",
                LastName = "Gurney",
                JoinDate = DateTime.Now
            };

            manager.Create(user, "TheBookOfLies1$$$");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("jordan.gurney@inContact.com");

            manager.AddToRoles(adminUser.Id, "User", "Admin");
        }
    }
}