using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class StaffWork
    {
        public int ID { get; set; }

        public string Type { get; set; }

        public ICollection<Staff> Staffs { get; set; }
    }
}
