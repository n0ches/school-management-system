using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystemManagement.Models
{
    public class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Birthday { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

    }
}
