using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class University
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Faculty> Faculties { get; set; }

        public ICollection<Staff> Staffs { get; set; }
    }
}
