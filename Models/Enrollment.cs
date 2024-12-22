using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class Enrollment : BaseEntity
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; } = null!;

        public virtual PaymentPlan? PaymentPlan { get; set; }
    }
}
