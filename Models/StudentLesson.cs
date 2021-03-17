using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public enum Grade
    {
        AA, BA, BB, CB, CC, DC, DD, FD, FF
    }

    public class StudentLesson
    {
        public int ID { get; set; }

        public Grade? Grade { get; set; }

        public Student Student { get; set; }

        public Lesson Lesson { get; set; }

    }
}
