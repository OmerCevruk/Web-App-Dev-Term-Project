using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class SportsCenter : BaseEntity
    {
        [Key]
        public int CenterId { get; set; }

        [Required]
        [StringLength(100)]
        public string CenterName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
