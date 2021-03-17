using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class Staff : Person
    {
        public int ID { get; set; }

        public University University { get; set; }

        public StaffWork StaffWork { get; set; }

        public int? Salary { get; set; }

    }
}
