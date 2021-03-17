using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public class Lesson
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int LessonPerWeek { get; set; }

        public int Credit { get; set; }

        public Lecturer Lecturer { get; set; }

        public Department Department { get; set; }

        public ICollection<StudentLesson> StudentLessons { get; set; }

    }
}
