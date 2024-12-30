using AthleteTracker.Models;
using AthleteTracker.Data;
using Microsoft.EntityFrameworkCore;


namespace AthleteTracker.Services
{

    public interface IPaymentService
    {
        Task<PaymentPlan> GeneratePaymentPlan(int enrollmentId, decimal totalAmount, int numberOfInstallments, int paymentDay);
        Task<bool> ProcessPayment(int paymentId, string paymentMethod);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ApplicationDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaymentPlan> GeneratePaymentPlan(int enrollmentId, decimal totalAmount,
            int numberOfInstallments, int paymentDay)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
                throw new ArgumentException("Enrollment not found");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create payment plan
                var plan = new PaymentPlan
                {
                    EnrollmentId = enrollmentId,
                    TotalAmount = totalAmount,
                    NumberOfInstallments = numberOfInstallments,
                    PaymentDay = paymentDay
                };

                _context.PaymentPlans.Add(plan);
                await _context.SaveChangesAsync();

                // Calculate monthly amount
                var monthlyAmount = Math.Round(totalAmount / numberOfInstallments, 2);
                var remainingAmount = totalAmount;

                // Generate individual payments
                for (int i = 0; i < numberOfInstallments; i++)
                {
                    decimal installmentAmount;
                    if (i == numberOfInstallments - 1)
                    {
                        installmentAmount = remainingAmount;
                    }
                    else
                    {
                        installmentAmount = monthlyAmount;
                        remainingAmount -= monthlyAmount;
                    }

                    var payment = new Payment
                    {
                        PlanId = plan.PlanId,
                        Amount = installmentAmount,
                        DueDate = DateTime.UtcNow.AddMonths(i).Date.AddDays(paymentDay - 1),
                        Status = PaymentStatus.Pending
                    };

                    _context.Payments.Add(payment);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return plan;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error generating payment plan for enrollment {EnrollmentId}", enrollmentId);
                throw;
            }
        }

        public async Task<bool> ProcessPayment(int paymentId, string paymentMethod)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                return false;

            payment.Status = PaymentStatus.Paid;
            payment.PaymentMethod = paymentMethod;
            payment.PaymentDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
