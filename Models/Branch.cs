using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class Branch : BaseEntity
    {
        [Key]
        public int BranchId { get; set; }

        [Required]
        public int CenterId { get; set; }

        [Required]
        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [ForeignKey("CenterId")]
        public virtual SportsCenter Center { get; set; } = null!;

        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<InstructorBranch> InstructorBranches { get; set; } = new List<InstructorBranch>();
    }
}

