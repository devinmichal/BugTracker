using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        // primary key
        public int Id { get; set; }
        // foreign keys
        public int TicketId { get; set; }
        public string ChangedById { get; set; }
        //properties
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }


        public virtual ApplicationUser ChangedBy { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}