using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class Department
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public bool? PrimaryEducation { get; set; }

        public bool? SecondaryEducation { get; set; }

        public Faculty Faculty { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Lecturer> Lecturers { get; set; }

        public ICollection<Lesson> Lessons { get; set; }

    }
}
