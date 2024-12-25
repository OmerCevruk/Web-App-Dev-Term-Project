using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class BranchCreateViewModel
    {
        [Required(ErrorMessage = "Branch name is required")]
        [Display(Name = "Branch Name")]
        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sports Center is required")]
        [Display(Name = "Sports Center")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // For dropdown list
        public List<SportsCenterViewModel> AvailableCenters { get; set; } = new();
    }

    public class SportsCenterViewModel
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; } = string.Empty;
    }
}
