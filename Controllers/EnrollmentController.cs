using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;
using AthleteTracker.Services;

namespace AthleteTracker.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentService _studentService;

        public EnrollmentController(
            ApplicationDbContext context,
            IStudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? studentId)
        {
            if (!studentId.HasValue)
            {
                var selectionViewModel = await _studentService.GetStudentSelectionList(
                    "Enrollment",
                    "Create",
                    "Create Enrollment");

                return View("~/Views/Shared/SelectStudent.cshtml", selectionViewModel);
            }

            var student = await _studentService.GetStudentById(studentId.Value);
            if (student == null)
                return NotFound();

            var availableSessions = await _context.Sessions
                .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
                .Where(s => s.Status == SessionStatus.Active)
                .Select(s => new SessionViewModel
                {
                    SessionId = s.SessionId,
                    SessionName = s.SessionName,
                    InstructorName = $"{s.Instructor.User.FirstName} {s.Instructor.User.LastName}",
                    Schedule = $"{s.DayOfWeek}: {s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}",
                    CurrentEnrollments = s.Enrollments.Count(e => e.IsActive),
                    MaxCapacity = s.MaxCapacity
                })
                .ToListAsync();

            var enrollmentViewModel = new EnrollmentViewModel
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                AvailableSessions = availableSessions
            };

            return View(enrollmentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload available sessions
                model.AvailableSessions = await GetAvailableSessions();
                return View(model);
            }

            // Verify session has capacity
            var session = await _context.Sessions
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                .FirstOrDefaultAsync(s => s.SessionId == model.SessionId);

            if (session == null)
            {
                ModelState.AddModelError("", "Selected session not found.");
                model.AvailableSessions = await GetAvailableSessions();
                return View(model);
            }

            if (session.Enrollments.Count >= session.MaxCapacity)
            {
                ModelState.AddModelError("", "This session has reached its maximum capacity.");
                model.AvailableSessions = await GetAvailableSessions();
                return View(model);
            }

            // Check for existing active enrollment
            var existingEnrollment = await _context.Enrollments
                .AnyAsync(e => e.StudentId == model.StudentId
                              && e.SessionId == model.SessionId
                              && e.IsActive);

            if (existingEnrollment)
            {
                ModelState.AddModelError("", "Student is already enrolled in this session.");
                model.AvailableSessions = await GetAvailableSessions();
                return View(model);
            }

            var enrollment = new Enrollment
            {
                StudentId = model.StudentId,
                SessionId = model.SessionId,
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                IsActive = true
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Enrollment created successfully.";
            return RedirectToAction("ManagePaymentPlan", "Payment", new { studentId = model.StudentId });
        }

        private async Task<List<SessionViewModel>> GetAvailableSessions()
        {
            return await _context.Sessions
                .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
                .Where(s => s.Status == SessionStatus.Active)
                .Select(s => new SessionViewModel
                {
                    SessionId = s.SessionId,
                    SessionName = s.SessionName,
                    InstructorName = $"{s.Instructor.User.FirstName} {s.Instructor.User.LastName}",
                    Schedule = $"{s.DayOfWeek}: {s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}",
                    CurrentEnrollments = s.Enrollments.Count(e => e.IsActive),
                    MaxCapacity = s.MaxCapacity
                })
                .ToListAsync();
        }
    }
}
