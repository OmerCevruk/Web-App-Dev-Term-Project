using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class PaymentPlan : BaseEntity
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        public int EnrollmentId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public int NumberOfInstallments { get; set; }

        [Required]
        public int PaymentDay { get; set; }

        [ForeignKey("EnrollmentId")]
        public virtual Enrollment Enrollment { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
