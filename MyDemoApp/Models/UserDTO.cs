using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
   public class UserDTO
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name:")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name:")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date:")]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Email Address:")]
        public string Email { get; set; }
    }
}
