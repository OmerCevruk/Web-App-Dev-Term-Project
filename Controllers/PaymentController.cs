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
        private readonly IPaymentService _paymentService;

        public PaymentController(
            ApplicationDbContext context,
            IStudentService studentService,
            IPaymentService paymentService)
        {
            _context = context;
            _studentService = studentService;
            _paymentService = paymentService;
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
                .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                .ThenInclude(e => e.Session)
                .Where(p => p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId))
                .OrderByDescending(p => p.DueDate)
                .Select(p => new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    DueDate = p.DueDate,
                    Status = p.Status,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    SessionName = p.Plan.Enrollment.Session.SessionName
                })
                .ToListAsync();

            var viewModel = new PaymentListViewModel
            {
                Payments = payments
            };

            return View(viewModel);
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePaymentPlan(PaymentPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var enrollment = await _context.Enrollments
                        .FirstOrDefaultAsync(e => e.StudentId == model.StudentId && e.IsActive);

                    if (enrollment == null)
                    {
                        ModelState.AddModelError("", "No active enrollment found for this student.");
                        return View("ManagePaymentPlan", model);
                    }

                    await _paymentService.GeneratePaymentPlan(
                        enrollment.EnrollmentId,
                        model.TotalAmount,
                        model.NumberOfInstallments,
                        model.PaymentDay);

                    TempData["Success"] = "Payment plan created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating payment plan. Please try again. {}");
                    Console.WriteLine(ex);
                }
            }

            return View("ManagePaymentPlan", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int paymentId, string paymentMethod)
        {
            if (await _paymentService.ProcessPayment(paymentId, paymentMethod))
            {
                TempData["Success"] = "Payment processed successfully.";
            }
            else
            {
                TempData["Error"] = "Error processing payment. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManagementDashboard()
        {
            var payments = await _context.Payments
                .Include(p => p.Plan)
                  .ThenInclude(p => p.Enrollment)
                  .ThenInclude(e => e.Student)
                  .ThenInclude(s => s.Parent)
                  .ThenInclude(p => p.User)
                    .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                  .ThenInclude(e => e.Session)
                    .OrderBy(p => p.DueDate)
                .ToListAsync();

            return View(payments);
        }

        // make payment actions
        [Authorize]
        public async Task<IActionResult> StudentPayments(int studentId)
        {
            //  if parent has Student
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Challenge();

            var student = await _context.Students
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(s => s.StudentId == studentId
                    && s.Parent.UserId == int.Parse(userId));

            if (student == null)
                return NotFound();

            var payments = await _context.Payments
                .Include(p => p.Plan)
                    .ThenInclude(p => p.Enrollment)
                        .ThenInclude(e => e.Session)
                .Where(p => p.Plan.Enrollment.StudentId == studentId)
                .OrderBy(p => p.DueDate)
                .Select(p => new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    DueDate = p.DueDate,
                    Status = p.Status,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    SessionName = p.Plan.Enrollment.Session.SessionName
                })
                .ToListAsync();

            var viewModel = new PaymentListViewModel
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                Payments = payments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> MakePayment(int paymentId, string paymentMethod)
        {
            // Verify parent has access to this payment
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Challenge();

            var payment = await _context.Payments
                .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId
                    && p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId));

            if (payment == null)
                return NotFound();

            if (await _paymentService.ProcessPayment(paymentId, paymentMethod))
            {
                TempData["Success"] = "Payment processed successfully.";
            }
            else
            {
                TempData["Error"] = "Error processing payment. Please try again.";
            }

            return RedirectToAction(nameof(StudentPayments),
                new { studentId = payment.Plan.Enrollment.StudentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> PayRemaining(string paymentMethod)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Challenge();

            var pendingPayments = await _context.Payments
                .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                .ThenInclude(e => e.Student)
                .Where(p => p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId) &&
                      (p.Status == PaymentStatus.Pending || p.Status == PaymentStatus.Overdue))
                .ToListAsync();

            if (!pendingPayments.Any())
            {
                TempData["Info"] = "No pending payments found.";
                return RedirectToAction(nameof(Index));
            }

            var successCount = 0;
            foreach (var payment in pendingPayments)
            {
                if (await _paymentService.ProcessPayment(payment.PaymentId, paymentMethod))
                {
                    successCount++;
                }
            }

            if (successCount > 0)
            {
                TempData["Success"] = $"Successfully processed {successCount} payment(s).";
            }
            else
            {
                TempData["Error"] = "Failed to process payments. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
