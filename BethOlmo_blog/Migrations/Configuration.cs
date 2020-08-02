namespace BethOlmo_blog.Migrations
{
    using BethOlmo_blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BethOlmo_blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BethOlmo_blog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>( 
                new RoleStore<IdentityRole>(context));

            //Check whether a particular Role already exists in the DB.
            //If not, create it.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
               roleManager.Create(new IdentityRole { Name = "Admin"});
            }

            if (!context.Roles.Any(r => r.Name == "Mod"))
            {
                roleManager.Create(new IdentityRole { Name = "Mod" });
            }

            //Create a user
            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));

            //Check whether a user exists with a specific Email
            //If NOT, create one.
            if (!context.Users.Any(u => u.Email == "betholmo@gmail.com"))
            {

                userManager.Create(new ApplicationUser()
                {
                    Email = "betholmo@gmail.com",
                    UserName = "betholmo@gmail.com",
                    FirstName = "Beth",
                    LastName = "Olmo",
                    DisplayName = "Poutrelli"
                    }, "OkiMomo06!");

                //Get the newly created Id
                var userId = userManager.FindByEmail("betholmo@gmail.com").Id;

                //Assign the new user to a specific Role
                userManager.AddToRole(userId, "Admin");

                //Create the user to occupy the Moderator Role
                //Assign that user to the Moderator Role
            }

            #region Seeding Blog Posts - commented out
            //for (var loop = 1; loop<=30; loop++)
            //{
            //    context.BlogPosts.AddOrUpdate(
            //        b => b.Title,
            //        new BlogPost
            //        {
            //            Title = $"Seeded Title {loop}",
            //            Body = $"Seeded Body {loop}",
            //            Abstract = $"Seeded Abstract {loop}",
            //            Created = DateTime.Now
            //        });
            //}
            #endregion


            #region Loading up categories
            context.Categories.AddOrUpdate(
                b => b.Name,
                    new Category { Name = "C#.Net", Description = "Working with C#" },
                    new Category { Name = "JavaScript", Description = "Working with JavaScript" },
                    new Category { Name = "Bootstrap", Description = "Design and layout" },
                    new Category { Name = "jQuery", Description = "Working with jQuery" },
                    new Category { Name = "LINQ", Description = "Working with LINQ" },
                    new Category { Name = "Career and Life", Description = "All about the journey" });
            #endregion

            if (!context.Users.Any(u => u.Email == "jasontwichell@coderfoundry.com"))
            {

                userManager.Create(new ApplicationUser()
                {
                    Email = "jasontwichell@coderfoundry.com",
                    UserName = "jasontwichell@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "The Prof"
                }, "BthIsGr8Stdt!");

                //Get the newly created Id
                var userId = userManager.FindByEmail("jasontwichell@coderfoundry.com").Id;

                //Assign the new user to a specific Role
                userManager.AddToRole(userId, "Mod");

                //Create the user to occupy the Moderator Role
                //Assign that user to the Moderator Role
            }
        }
    }
}
