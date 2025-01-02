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
                .Select(s => new StudentViewModel
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Enrollments = s.Enrollments.Select(e => new EnrollmentViewModel
                    {
                        StudentId = e.StudentId,
                        SessionId = e.SessionId,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        AvailableSessions = new List<SessionViewModel>
                        {
                    new SessionViewModel
                    {
                        SessionId = e.Session.SessionId,
                        SessionName = e.Session.SessionName,
                        InstructorName = $"{e.Session.Instructor.User.FirstName} {e.Session.Instructor.User.LastName}",
                        Schedule = $"{e.Session.DayOfWeek}: {e.Session.StartTime:hh\\:mm} - {e.Session.EndTime:hh\\:mm}",
                        CurrentEnrollments = e.Session.Enrollments.Count(en => en.IsActive),
                        MaxCapacity = e.Session.MaxCapacity
                    }
                        }
                    }).ToList()
                })
                .ToListAsync();

            var upcomingPayments = await _context.Payments
                .Include(p => p.Plan)
                    .ThenInclude(p => p.Enrollment)
                        .ThenInclude(e => e.Session)
                .Where(p => p.Plan.Enrollment.Student.Parent.UserId == int.Parse(userId) &&
                           p.Status == PaymentStatus.Pending &&
                           p.DueDate <= DateTime.Today.AddDays(30))
                .OrderBy(p => p.DueDate)
                .Take(5)
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

            var viewModel = new ParentDashboardViewModel
            {
                Students = students,
                UpcomingPayments = upcomingPayments
            };

            return View(viewModel);
        }
    }
}
