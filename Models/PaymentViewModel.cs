using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class PaymentPlanViewModel
    {
        public int StudentId { get; set; }

        [Required]
        [Range(1, 24)]
        [Display(Name = "Number of Installments")]
        public int NumberOfInstallments { get; set; }

        [Required]
        [Range(1, 31)]
        [Display(Name = "Payment Day")]
        public int PaymentDay { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }
    }
}
