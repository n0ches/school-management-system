using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    public enum Rank
    {
        Prof, Assoc_Prof_Dr, Asst_Prof_Dr, Dr, Asst, Lecturer 
    }
    public class Lecturer : Person
    {

        public int ID { get; set; }

        public Rank Rank { get; set; }

        public int Salary { get; set; }

        public Department Department { get; set; }

        public ICollection<Lesson> Lessons { get; set; }



    }
}
