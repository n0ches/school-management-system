using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class Student : Person
    {

        public int ID { get; set; }

        public int SchoolNumber { get; set; }

        public int Class { get; set; }

        public Department Department { get; set; }

        public ICollection<StudentLesson> StudentLessons { get; set; }


    }
}
