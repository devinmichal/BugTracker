using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        //primary key
        public int Id { get; set; }
        //foreign key
        public int TicketId { get; set; }
        public string  UserId { get; set; }
        // property
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
       

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}