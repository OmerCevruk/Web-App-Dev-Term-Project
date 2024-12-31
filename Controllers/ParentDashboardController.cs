using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    [Authorize(Roles = "Parent")]
    public class ParentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Challenge();

            var students = await _context.Students
                .Include(s => s.Parent)
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                .ThenInclude(e => e.Session)
                .Where(s => s.Parent.UserId == int.Parse(userId))
                .ToListAsync();

            // Get upcoming payments
            var upcomingPayments = await _context.Payments
                .Include(p => p.Plan)
                .ThenInclude(p => p.Enrollment)
                .ThenInclude(e => e.Student)
                .Where(p => p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId) &&
                           p.Status == PaymentStatus.Pending &&
                           p.DueDate <= DateTime.Today.AddDays(30))
                .OrderBy(p => p.DueDate)
                .Take(5)
                .ToListAsync();

            var viewModel = new ParentDashboardViewModel
            {
                Students = students,
                UpcomingPayments = upcomingPayments
            };

            return View(viewModel);
        }
    }
}
