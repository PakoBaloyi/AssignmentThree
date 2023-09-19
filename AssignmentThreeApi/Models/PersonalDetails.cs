using System.ComponentModel.DataAnnotations;

namespace AssignmentThreeApi.Models
{
    public class PersonalDetails
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telephone is required.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Identity number is required.")]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "ID number must be 13 digits.")]
        public string IdentityNumber { get; set; }
    }
}
