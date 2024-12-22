using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Overdue,
        Cancelled
    }

    public class Payment : BaseEntity
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [ForeignKey("PlanId")]
        public virtual PaymentPlan Plan { get; set; } = null!;
    }
}
