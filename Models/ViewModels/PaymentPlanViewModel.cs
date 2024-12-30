using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class PaymentPlanViewModel
    {
        public int StudentId { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Number of installments must be between 1 and 24")]
        [Display(Name = "Number of Installments")]
        public int NumberOfInstallments { get; set; }

        [Required]
        [Range(1, 31, ErrorMessage = "Payment day must be between 1 and 31")]
        [Display(Name = "Payment Day of Month")]
        public int PaymentDay { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Total amount must be greater than 0")]
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Student Name")]
        public string? StudentName { get; set; }
    }
}
