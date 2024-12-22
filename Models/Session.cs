using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public enum SessionStatus
    {
        Active,
        Cancelled,
        Completed,
        OnHold
    }

    public class Session : BaseEntity
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionName { get; set; } = string.Empty;

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        [Required]
        public SessionStatus Status { get; set; } = SessionStatus.Active;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;

        [ForeignKey("InstructorId")]
        public virtual Instructor Instructor { get; set; } = null!;

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
