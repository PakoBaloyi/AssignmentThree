using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace AssignmentThreeApi.Models
{
    public class PersonalDetails
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string? firstName { get; set; }
        [Required]
        public string? lastName { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? telephone { get; set; }
        [Required]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "ID number must be 13 digits.")]
        public string? identityNumber{ get; set; }
    
    }
}
