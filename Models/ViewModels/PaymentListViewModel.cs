namespace AthleteTracker.Models
{
    public class PaymentListViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<PaymentViewModel> Payments { get; set; } = new();
    }

    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public PaymentStatus Status { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string SessionName { get; set; } = string.Empty;
    }

}
