using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class StudentRegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }

        [Display(Name = "Medical Conditions")]
        public string? MedicalConditions { get; set; }

        [Required(ErrorMessage = "Parent's email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Parent's Email")]
        public string ParentEmail { get; set; } = string.Empty;
    }
}
