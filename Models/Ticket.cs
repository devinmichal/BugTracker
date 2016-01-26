using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class Ticket
    {
        public Ticket ()
        {
            this.TicketComments = new HashSet<TicketComment>();
        }
        //primary key
        public int Id { get; set; } 
        //foreign keys
        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int TypeId { get; set; }
        public string OwnerId { get; set; }
        public string AssignedUserId { get; set; }
       
    
        // REQUIRED properties
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        //properties
        public System.DateTimeOffset Created { get; set; }
        public System.DateTimeOffset Updated { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
        [ForeignKey("AssignedUserId")]
        public virtual ApplicationUser AssignedUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual Status Status { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<TicketHistory> TicketHistory { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }


        

    }
}