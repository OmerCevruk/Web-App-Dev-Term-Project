using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;
using AthleteTracker.Services;

namespace AthleteTracker.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentService _studentService;

        public PaymentController(
            ApplicationDbContext context,
            IStudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        // GET: Payment/Index
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

        // GET: Payment/ManagePaymentPlan
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManagePaymentPlan(int? studentId)
        {
            if (!studentId.HasValue)
            {
                var selectionViewModel = await _studentService.GetStudentSelectionList(
                    "Payment",
                    "ManagePaymentPlan",
                    "Create Payment Plan");

                return View("~/Views/Shared/SelectStudent.cshtml", selectionViewModel);
            }

            var student = await _studentService.GetStudentById(studentId.Value);
            if (student == null)
                return NotFound();

            var paymentViewModel = new PaymentPlanViewModel
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}"
            };

            return View(paymentViewModel);
        }

        // POST: Payment/CreatePaymentPlan
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaymentPlan(PaymentPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var student = await _studentService.GetStudentById(model.StudentId);
                if (student != null)
                {
                    model.StudentName = $"{student.FirstName} {student.LastName}";
                }
                return View("ManagePaymentPlan", model);
            }

            // Get active enrollment for student
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == model.StudentId && e.IsActive);

            if (enrollment == null)
            {
                ModelState.AddModelError("", "No active enrollment found for this student.");
                return View("ManagePaymentPlan", model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create payment plan
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
                var monthlyAmount = Math.Round(model.TotalAmount / model.NumberOfInstallments, 2);
                var remainingAmount = model.TotalAmount;

                for (int i = 0; i < model.NumberOfInstallments; i++)
                {
                    decimal installmentAmount;
                    if (i == model.NumberOfInstallments - 1)
                    {
                        // Last payment accounts for any rounding differences
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
                        DueDate = DateTime.UtcNow.AddMonths(i).Date.AddDays(model.PaymentDay - 1),
                        Status = PaymentStatus.Pending
                    };

                    _context.Payments.Add(payment);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Payment plan created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while creating the payment plan. Please try again.");
                return View("ManagePaymentPlan", model);
            }
        }

        // POST: Payment/MakePayment
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
    }
}
