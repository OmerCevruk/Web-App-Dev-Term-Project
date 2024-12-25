using System.ComponentModel.DataAnnotations;
namespace AthleteTracker.Models
{
    public class SportsCenterCreateViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Center Name")]
        public string CenterName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
