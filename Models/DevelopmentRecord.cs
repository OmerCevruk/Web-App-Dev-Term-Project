using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class DevelopmentRecord : BaseEntity
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Height { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Weight { get; set; }

        public string? InstructorComments { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;
    }
}
