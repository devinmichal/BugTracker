namespace BugTracker.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BugTracker.Models.ApplicationDbContext";
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if(!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if(!context.Users.Any(u => u.Email == "DevinFeemster@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DevinFeemster@gmail.com",
                    Email = "DevinFeemster@gmail.com",
                    FirstName = "Devin",
                    LastName = "Feemster",
                    DisplayName = "Devin Feemster"
                }, "Password-1");
            }

         

            if (!context.Users.Any(u => u.Email == "GuestAdmin@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "GuestAdmin@gmail.com",
                    Email = "GuestAdmin@gmail.com",
                    FirstName = "Guest",
                    LastName = "Who",
                    DisplayName = "GuestAdmin"
                }, "Password-1");
            }

            if (!context.Users.Any(u => u.Email == "GuestPm@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "GuestPm@gmail.com",
                    Email = "GuestPm@gmail.com",
                    FirstName = "Guest",
                    LastName = "Pm",
                    DisplayName = "GuestPm"
                }, "Password-1");
            }

            if (!context.Users.Any(u => u.Email == "GuestSub@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "GuestSub@gmail.com",
                    Email = "GuestSub@gmail.com",
                    FirstName = "Guest",
                    LastName = "Sub",
                    DisplayName = "GuestSub"
                }, "Password-1");
            }


            if (!context.Users.Any(u => u.Email == "GuestDev@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "GuestDev@gmail.com",
                    Email = "GuestDev@gmail.com",
                    FirstName = "Guest",
                    LastName = "Dev",
                    DisplayName = "GuestDev"
                }, "Password-1");
            }


            var userId = userManager.FindByEmail("DevinFeemster@gmail.com").Id;
            var userId2 = userManager.FindByEmail("GuestAdmin@gmail.com").Id;
            var userId3 = userManager.FindByEmail("GuestDev@gmail.com").Id;
            var userId4 = userManager.FindByEmail("GuestSub@gmail.com").Id;
            var userId5 = userManager.FindByEmail("GuestPm@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            userManager.AddToRole(userId2, "Admin");
            userManager.AddToRole(userId3, "Developer");
            userManager.AddToRole(userId4, "Submitter");
            userManager.AddToRole(userId5, "Project Manager");



            if (!context.TicketTypes.Any(t=>t.Name == "Bug"))
            {
                context.TicketTypes.Add(new Models.Type() { Name = "Bug" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Feature Request"))
            {
                context.TicketTypes.Add(new Models.Type() { Name = "Feature Request" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Feature"))
            {
                context.TicketTypes.Add(new Models.Type() { Name = "Feature" });
            }
            context.SaveChanges();

            if (!context.TicketPriorities.Any(s => s.Name == "High"))
            {
                context.TicketPriorities.Add(new Models.Priority() { Name = "High" });
            }


            if (!context.TicketPriorities.Any(s => s.Name == "Medium"))
            {
                context.TicketPriorities.Add(new Models.Priority() { Name = "Medium" });
            }


            if (!context.TicketPriorities.Any(s => s.Name == "Low"))
            {
                context.TicketPriorities.Add(new Models.Priority() { Name = "Low" });
            }

            context.SaveChanges();

            if(!context.TicketStatuses.Any(s=> s.Name == "Resolved"))
            {
                context.TicketStatuses.Add(new Models.Status() { Name = "Resolved" });
            }

            if (!context.TicketStatuses.Any(s => s.Name == "Incomplete"))
            {
                context.TicketStatuses.Add(new Models.Status() { Name = "Incomplete" });
            }

            if (!context.TicketStatuses.Any(s => s.Name == "Unresolved"))
            {
                context.TicketStatuses.Add(new Models.Status() { Name = "Unresolved" });
            }







            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
