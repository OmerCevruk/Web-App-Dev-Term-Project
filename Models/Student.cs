using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class Student : BaseEntity
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public char Gender { get; set; }

        public string? MedicalConditions { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; } = null!;

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
