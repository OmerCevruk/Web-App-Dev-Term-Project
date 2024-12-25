using AthleteTracker.Models;
public class PaymentManagementViewModel
{
    public int EnrollmentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string SessionName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int NumberOfInstallments { get; set; }
    public int PaymentDay { get; set; }
    public List<PaymentDetailViewModel> Payments { get; set; } = new();
}

public class PaymentDetailViewModel
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string? PaymentMethod { get; set; }
}
