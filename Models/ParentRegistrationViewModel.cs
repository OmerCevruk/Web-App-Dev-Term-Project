using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class ParentRegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact is required")]
        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency phone is required")]
        [Phone(ErrorMessage = "Invalid emergency phone number")]
        [Display(Name = "Emergency Phone")]
        public string EmergencyPhone { get; set; } = string.Empty;
    }
}
