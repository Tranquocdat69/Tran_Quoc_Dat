using InternManagementMVC.Models;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace InternManagementAPI.Models
{
    public partial class TSchedule
    {
        public int AScheduleId { get; set; }
        public int? AStudentId { get; set; }
        public DateTime? AAttendedDate { get; set; }
        public int? ASession { get; set; }
        public DateTime? ACreatedDate { get; set; }
        public DateTime? AUpdateDate { get; set; }

    }
}
