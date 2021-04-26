using InternManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternManagementMVC.Models
{
    public class TStudent
    {
        public int AStudentId { get; set; }
        public string AUsername { get; set; }
        public string AFullName { get; set; }
        public string AEmail { get; set; }
        public DateTime? ACreatedDate { get; set; }
        public DateTime? AUpdatedDate { get; set; }
    }
}
