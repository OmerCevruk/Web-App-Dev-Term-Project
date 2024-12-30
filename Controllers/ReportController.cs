using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null)
        {
            var viewModel = new ReportViewModel
            {
                StartDate = startDate ?? DateTime.Today.AddMonths(-1),
                EndDate = endDate ?? DateTime.Today
            };

            // Convert dates to UTC for database comparison
            var startUtc = DateTime.SpecifyKind(viewModel.StartDate, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(viewModel.EndDate, DateTimeKind.Utc);
            // get student counts
            viewModel.TotalStudents = await _context.Students.CountAsync();
            viewModel.ActiveStudents = await _context.Students
                .Where(s => s.IsActive)
                .CountAsync();

            //get enrollment counts
            viewModel.NewEnrollments = await _context.Enrollments
                .Where(e => e.CreatedAt >= startUtc && e.CreatedAt <= endUtc)
                .CountAsync();

            viewModel.Withdrawals = await _context.Enrollments
                .Where(e => e.UpdatedAt >= startUtc && e.UpdatedAt <= endUtc && !e.IsActive)
                .CountAsync();

            //get paid 😎
            viewModel.TotalIncome = await _context.Payments
                .Where(p => p.PaymentDate >= startUtc && p.PaymentDate <= endUtc
                           && p.Status == PaymentStatus.Paid)
                .SumAsync(p => p.Amount);

            // get session statistics
            viewModel.SessionStats = await _context.Sessions
                .Include(s => s.Branch)
                .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
                .Include(s => s.Enrollments)
                .Where(s => s.Status == SessionStatus.Active)
                .Select(s => new SessionStatistics
                {
                    SessionName = s.SessionName,
                    BranchName = s.Branch.BranchName,
                    InstructorName = $"{s.Instructor.User.FirstName} {s.Instructor.User.LastName}",
                    CurrentEnrollments = s.Enrollments.Count(e => e.IsActive),
                    MaxCapacity = s.MaxCapacity
                })
                .ToListAsync();

            return View(viewModel);
        }

        // TODO: implement a way to export stats as cs
        [HttpGet]
        public async Task<IActionResult> Export(DateTime startDate, DateTime endDate)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
