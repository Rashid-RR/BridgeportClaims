using System;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Migrations
{
    using System.Data.Entity.Migrations;

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

            var user = new ApplicationUser
            {
                UserName = "jordangurney@gmail.com",
                Email = "jordangurney@gmail.com",
                EmailConfirmed = true,
                FirstName = "Jordan",
                LastName = "Gurney",
                JoinDate = DateTime.Now
            };

            manager.Create(user, "TheBookOfLies1$");
        }
    }
}
