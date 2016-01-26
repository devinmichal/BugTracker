using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helper;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       

        // GET: TicketModels
        public ActionResult Index()
        {
            var Ticket = db.Tickets.ToList();
            var user = db.Users.Find(User.Identity.GetUserId());

            if (User.IsInRole("Admin") && User.IsInRole("Developer") && User.IsInRole("Project Manager"))

            {
                return PartialView("_Index",Ticket);
            }

            if (User.IsInRole("Admin") && User.IsInRole("Project Manager"))

            {
                return PartialView("_Index",Ticket);
            }

            if (User.IsInRole("Admin") && User.IsInRole("Developer"))

            {
                return PartialView("_Index",Ticket);
            }

            if (User.IsInRole("Project Manager") || User.IsInRole("Developer"))
            {
                var UserTickets = user.Projects.SelectMany(p => p.Tickets).ToList();

                return PartialView("_Index",UserTickets);
            }

            if (User.IsInRole("Submitter"))
            {
                var UserTickets = db.Tickets.Where(t => t.OwnerId == user.Id).ToList();

                return PartialView("_Index",UserTickets);
            }

            return PartialView("_Index",Ticket);
        }

        // GET: TicketModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticketModel = db.Tickets.Find(id);
            if (ticketModel == null)
            {
                return HttpNotFound();
            }
            return View(ticketModel);
        }

        // GET: Tickets1/Create
        public ActionResult Create()
        {
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id","DisplayName");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                ticket.Created = DateTimeOffset.Now;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ticket.Id});
            }

            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedUserId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }


        // GET: TicketModels/Edit/5
        public ActionResult Edit(int? id)
        {
            UserRoleHelper helper = new UserRoleHelper();
            var devs = helper.UsersInRole("Developer").ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticketModel = db.Tickets.Find(id);
            if (ticketModel == null)
            {
                return HttpNotFound();
            }


            ViewBag.AssignedUserId = new SelectList(devs, "Id", "DisplayName", ticketModel.Id);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticketModel.OwnerId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticketModel.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketModel.ProjectId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticketModel.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticketModel.TypeId);
            return View(ticketModel);
        }

        // POST: TicketModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ticket ticket)
        {
           
             var user = db.Users.Find(User.Identity.GetUserId());
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
            if (ModelState.IsValid)
            {
                if (oldTicket.Body != ticket.Body)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Body;
                    history.NewValue = ticket.Body;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the body of the ticket!";
                    db.TicketNotifications.Add(notification);

                }

                if (oldTicket.Title != ticket.Title)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Title;
                    history.NewValue = ticket.Title;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the title of the ticket!";
                    db.TicketNotifications.Add(notification);

                }

                //if (oldTicket.AssignedUserId != ticket.AssignedUserId)
                //{
                //    var notification = new TicketNotification();
                //    var history = new TicketHistory();
                //    history.OldValue = oldTicket.AssignedUser.DisplayName;
                //    history.NewValue = db.Tickets.Find(ticket.Id).AssignedUser.DisplayName;
                //    history.ChangedById = User.Identity.GetUserId();
                //    history.TicketId = ticket.Id;
                //    db.TicketHistories.Add(history);
                //    notification.UserId = ticket.AssignedUserId;
                //    notification.TicketId = ticket.Id;
                //    db.TicketNotifications.Add(notification);
                //}

                if (oldTicket.PriorityId != ticket.PriorityId)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Priority.Name;
                    history.NewValue = db.TicketPriorities.Find(ticket.PriorityId).Name;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the priority of the ticket!";
                    db.TicketNotifications.Add(notification);
                }



                if (oldTicket.ProjectId != ticket.ProjectId)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Project.Name;
                    history.NewValue = db.Projects.Find(ticket.ProjectId).Name;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the project of the ticket!";
                    db.TicketNotifications.Add(notification);
                }

                if (oldTicket.StatusId != ticket.StatusId)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Status.Name;
                    history.NewValue = db.TicketStatuses.Find(ticket.StatusId).Name;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the status of the ticket!";
                    db.TicketNotifications.Add(notification);
                }

                if (oldTicket.TypeId != ticket.TypeId)
                {
                    var notification = new TicketNotification();
                    var history = new TicketHistory();
                    history.OldValue = oldTicket.Type.Name;
                    history.NewValue = db.TicketTypes.Find(ticket.TypeId).Name;
                    history.ChangedById = User.Identity.GetUserId();
                    history.TicketId = ticket.Id;
                    db.TicketHistories.Add(history);
                    notification.UserId = ticket.AssignedUserId;
                    notification.TicketId = ticket.Id;
                    notification.Content =  "changed the type of the ticket!";
                    db.TicketNotifications.Add(notification);

                }

                db.Tickets.Add(ticket);
                ticket.Updated = DateTimeOffset.Now;

                db.Entry(ticket).State = EntityState.Modified;

                db.SaveChanges();
           
            }
            return RedirectToAction("Details", new { id = ticket.Id });
        }



        [HttpPost]
        public ActionResult SubmitComment(TicketComment comment, HttpPostedFileBase image, int TicketId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var ticket = db.Tickets.Find(TicketId);
            if (image != null && image.ContentLength > 0)
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format.");
                }
                if (image != null)
                {
                    //relative server path
                    var filePath = "/Uploads/";
                    //path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);
                    //media url for relative path
                    comment.MediaURl = filePath + image.FileName;
                    Directory.CreateDirectory(absPath);
                    //save image
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
            }

            if (ModelState.IsValid)
            {
                comment.Created = DateTimeOffset.Now;
                var notification = new TicketNotification();
                db.TicketComments.Add(comment);
                ticket.TicketComments.Add(comment);
                notification.UserId = ticket.AssignedUserId;
                notification.TicketId = ticket.Id;
                notification.Content =  "left a comment on ticket!"; 
                db.TicketNotifications.Add(notification);

                db.SaveChanges();
            }
          
            return RedirectToAction("Details",ticket);
        }


        
        public ActionResult DeleteNotification(int? Id)
        {
            var noti = db.TicketNotifications.Find(Id);
            noti.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index","Admin");
        }
        
        public PartialViewResult Tickets ()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(t => t.AssignedUserId == currentUser.Id).ToList();
            return PartialView("_TicketCount",tickets);
        }

        public PartialViewResult TicketComments ()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(t => t.AssignedUserId == currentUser.Id);

            var comments = from c in tickets
                           
                           select c.TicketComments;
                           

            
            return PartialView("_TicketComments", comments.ToList());
        }


    }

    //[HttpPost]
    //public ActionResult EditComment(Comment comment, string Slug)
    //{
    //    BlogPost post = db.Posts.FirstOrDefault(p => p.Slug == Slug);
    //    db.Comments.Attach(comment);
    //    comment.Updated = System.DateTimeOffset.Now.Date;
    //    db.Entry(comment).Property("Body").IsModified = true;
    //    db.SaveChanges();

    //    return RedirectToAction("Display", post);
    //}


}

