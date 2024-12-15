using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class Parent : BaseEntity
    {
        [Key]
        public int ParentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string EmergencyPhone { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
