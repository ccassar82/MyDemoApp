using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsersAPI
{
    ///<Summary>
    /// This class inherits from IValidatableObject as to be able to perform validation within API calls
    ///</Summary>
    public class UserDTO : IValidatableObject
    {
        public Guid? Id { get; set; }

        [Required, StringLength(50, ErrorMessage = "First Name can only be 50 characters long")]
        public string Name { get; set; }

        [Required, StringLength(50, ErrorMessage = "Last Name can only be 50 characters long")]
        public string Surname { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required, StringLength(50, ErrorMessage = "Email address can only be 50 characters long")]
        public string Email { get; set; }

        bool IsValidDate(DateTime dateTime)
        {
            return dateTime <= DateTime.Today;
        }

        bool IsDatePopulated(DateTime dateTime)
        {
            return dateTime != new DateTime();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!IsValidDate(this.BirthDate))
            {
                results.Add(new ValidationResult("Birth Date cannot be in the future"));
            }
            if (!IsDatePopulated(this.BirthDate))
            {
                results.Add(new ValidationResult("The Birth Date field is required"));
            }

            return results;
        }
    }
}
