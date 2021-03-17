using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class Faculty
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public University University { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
