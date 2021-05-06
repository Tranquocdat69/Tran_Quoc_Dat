using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace InternManagementAPI.Models
{
    public partial class TStudents
    {
        public TStudents()
        {
            TSchedule = new HashSet<TSchedule>();
        }

        public int AStudentId { get; set; }
        public string AUsername { get; set; }
        public string AFullName { get; set; }
        public string AEmail { get; set; }
        [JsonIgnore]
        public DateTime? ACreatedDate { get; set; }
        [JsonIgnore]
        public DateTime? AUpdatedDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<TSchedule> TSchedule { get; set; }
    }
}
