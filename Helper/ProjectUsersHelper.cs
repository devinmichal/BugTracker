using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using BugTracker.Helper;
using System.Web.Mvc;

namespace BugTracker.Helper
{
    public class ProjectUsersHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

       public Project FindProject (int Project)
        {
           return db.Projects.Find(Project);
        }

        public bool IsOnProject( ApplicationUser User, int Project)
        {
            var project = FindProject(Project);
            if (project.Users.Contains(db.Users.Find(User.Id)))
            {
                return true;
            }

            return false;
        }

        public bool ProjectHasUsers (int Project)
        {
            Project project = FindProject(Project);

            return project.Users.Any();
        }

        public bool ProjectHasPm(int Project)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var pms = helper.UsersInRole("Project Manager");
           
            var project = FindProject(Project);

            foreach(var p in pms)
            {
                if(project.Users.Contains(p))
                {
                    return true;
                }
            }
            return false;
        }

        public ApplicationUser ProjectPm(int Project)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var pms = helper.UsersInRole("Project Manager");

            Project project = FindProject(Project);

            foreach (var p in pms)
            {
                if (project.Users.Contains(p))
                {
                    return p;
                }
            }
            return null; ;
        }

        public bool ProjectHasDev(int Project)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var dev = helper.UsersInRole("Developer");

            Project project = FindProject(Project);

            foreach (var d in dev)
            {
                if (project.Users.Contains(d))
                {
                    return true;
                }
            }
            return false;
        }

        public ApplicationUser ProjectDev(int Project)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var dev = helper.UsersInRole("Developer");

            Project project = FindProject(Project);

            foreach (var d in dev)
            {
                if (project.Users.Contains(d))
                {
                    return d;
                }
            }
            return null; ;
        }

        public List<ApplicationUser> ListProjectUsers (int Project)
        {
            Project project = FindProject(Project);

            return project.Users.ToList();
        }
        
        public void AssignUserToProject (ProjectUserViewModel model)
        {
            Project project = FindProject(model.Project.Id);
            project.Users.Clear();
            project.Users = db.Users.Where(u => model.SelectedUsers.Contains(u.Id)).ToList();
            db.SaveChanges();
        }

        public void RemoveUserToProject(int Project, string User)
        {
            Project project = FindProject(Project);
            ApplicationUser user = db.Users.Find(User);
            project.Users.Remove(user);
            db.SaveChanges();
        }

    }
}