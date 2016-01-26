using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Users ()
        {

            return PartialView("_UserCount", db.Users.ToList());
        }

        public PartialViewResult UserNav()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            return PartialView("_Nav", currentUser);
        }
    }
}