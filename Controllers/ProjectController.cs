using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helper;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class ProjectController : Controller
    {
        //instantiaing helper
        private ProjectUsersHelper PHelper = new ProjectUsersHelper();
        //instantiating the DB
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Project
        public ActionResult Index()
        {
            var Projects = db.Projects.AsQueryable();

            var user = db.Users.Find(User.Identity.GetUserId());

            if(User.IsInRole("Admin") && User.IsInRole("Developer") && User.IsInRole("Project Manager")) 
                
                {
                return PartialView("_Index",Projects.ToList());
            }

            if (User.IsInRole("Admin") && User.IsInRole("Project Manager"))

                {
                return PartialView("_Index",Projects.ToList());
            }

            if (User.IsInRole("Admin") && User.IsInRole("Developer"))

                {
                return PartialView("_Index",Projects.ToList());
            }




            if (User.IsInRole("Developer") || User.IsInRole("Project Manager"))
            {
                
                Projects = user.Projects.AsQueryable();
                
            }
            return PartialView("_Index",Projects.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Project Project)
        {
            db.Projects.Add(Project);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

      
  
        public ActionResult Edit(int? Id)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var pms = helper.UsersInRole("Project Manager");
            var devs = helper.UsersInRole("Developer");
            var project = db.Projects.Find(Id);
            var model = new ProjectUserViewModel()
            {
                Project = project,
               Pms = new MultiSelectList(pms, "Id", "UserName", project.Users.Select(u => u.Id)),
                Devs = new MultiSelectList(devs, "Id", "UserName", project.Users.Select(u => u.Id)),
                Ticket = null,
               


               
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProjectUserViewModel model)
        {
            db.Projects.Attach(model.Project);
            db.Entry(model.Project).Property("Name").IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Edit", new { Id = model.Project.Id });
        }

        public PartialViewResult Projects()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var projects = currentUser.Projects;
            return PartialView("_ProjectsCount", projects);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult AssignPm(ProjectUserViewModel model)
        {
          
            PHelper.AssignUserToProject(model);
            return RedirectToAction("Edit", new{Id = model.Project.Id });
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles ="Project Manager")]
        [HttpPost]
        public ActionResult AssignDev(ProjectUserViewModel model)
        {
            PHelper.AssignUserToProject(model);
            return RedirectToAction("Edit", new { Id = model.Project.Id });
        }

    }
}