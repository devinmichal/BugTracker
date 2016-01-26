using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketComment
    {
        // primary key
        public int Id { get; set; }
        // foreign key
        public string UserId { get; set; }
        
        // properties
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        public System.DateTimeOffset Created { get; set; }
        public string MediaURl { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}