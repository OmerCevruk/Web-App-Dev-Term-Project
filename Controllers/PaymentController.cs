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

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return NotFound();

            var viewModel = new PaymentPlanViewModel
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}"
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaymentPlan(PaymentPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
                if (student != null)
                {
                    model.StudentName = $"{student.FirstName} {student.LastName}";
                }
                return View("ManagePaymentPlan", model);
            }

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
                var plan = new PaymentPlan
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    TotalAmount = model.TotalAmount,
                    NumberOfInstallments = model.NumberOfInstallments,
                    PaymentDay = model.PaymentDay
                };

                _context.PaymentPlans.Add(plan);
                await _context.SaveChangesAsync();

                var monthlyAmount = Math.Round(model.TotalAmount / model.NumberOfInstallments, 2);
                var remainingAmount = model.TotalAmount;

                for (int i = 0; i < model.NumberOfInstallments; i++)
                {
                    decimal installmentAmount;
                    if (i == model.NumberOfInstallments - 1)
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
    }
}
