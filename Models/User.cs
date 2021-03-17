using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManagement.Models
{
    

    public class User
    {
        public int ID { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage ="The username is required.")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }
        public int UserType { get; set; }
        public int person { get; set; }
    }
}
