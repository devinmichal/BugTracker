using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectUserViewModel
    {
        public Ticket Ticket { get; set; }
        public Project Project { get; set; }
        public MultiSelectList Pms { get; set; }
        public MultiSelectList Devs { get; set; }
        public IList<string> SelectedUsers { get; set; }


    }
}