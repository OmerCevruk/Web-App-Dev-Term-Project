using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // payment details
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Challenge();

            var payments = await _context.Payments
                .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                .ThenInclude(e => e.Student)
                .Where(p => p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId))
                .ToListAsync();

            return View(payments);
        }

        // payment plan
        public async Task<IActionResult> Plan(int studentId)
        {
            var plan = await _context.PaymentPlans
                .Include(p => p.Enrollment)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(p => p.Enrollment.StudentId == studentId);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        // make a payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayment(int paymentId, string paymentMethod)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                return NotFound();

            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = PaymentStatus.Paid;
            payment.PaymentMethod = paymentMethod;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "HR")]
        public async Task<IActionResult> ManagePaymentPlan(int? studentId)
        {
            if (!studentId.HasValue)
            {
                var students = await _context.Students
                    .Include(s => s.Parent)
                    .ThenInclude(p => p.User)
                    .Where(s => s.IsActive)
                    .Select(s => new StudentSelectViewModel
                    {
                        StudentId = s.StudentId,
                        FullName = $"{s.FirstName} {s.LastName}",
                        ParentName = $"{s.Parent.User.FirstName} {s.Parent.User.LastName}"
                    })
                    .ToListAsync();

                return View("SelectStudent", students);
            }

            var student = await _context.Students
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return NotFound();

            return View(new PaymentPlanViewModel { StudentId = student.StudentId });
        }

        [HttpPost]
        [Authorize(Policy = "HR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaymentPlan(PaymentPlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == model.StudentId && e.IsActive);

            if (enrollment == null)
                return NotFound();

            var plan = new PaymentPlan
            {
                EnrollmentId = enrollment.EnrollmentId,
                TotalAmount = model.TotalAmount,
                NumberOfInstallments = model.NumberOfInstallments,
                PaymentDay = model.PaymentDay
            };

            _context.PaymentPlans.Add(plan);
            await _context.SaveChangesAsync();

            // Create individual payments
            var monthlyAmount = model.TotalAmount / model.NumberOfInstallments;
            for (int i = 0; i < model.NumberOfInstallments; i++)
            {
                var payment = new Payment
                {
                    PlanId = plan.PlanId,
                    Amount = monthlyAmount,
                    DueDate = DateTime.UtcNow.AddMonths(i).Date.AddDays(model.PaymentDay - 1),
                    Status = PaymentStatus.Pending
                };
                _context.Payments.Add(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Plan), new { studentId = model.StudentId });
        }
    }
}
