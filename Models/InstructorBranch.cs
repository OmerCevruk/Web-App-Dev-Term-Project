using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class InstructorBranch : BaseEntity
    {
        [Key]
        public int InstructorBranchId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor Instructor { get; set; } = null!;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;
    }
}
