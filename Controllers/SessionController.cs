using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SessionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _context.Sessions
                .Include(s => s.Branch)
                .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .Select(s => new SessionIndexViewModel
                {
                    SessionId = s.SessionId,
                    SessionName = s.SessionName,
                    BranchName = s.Branch.BranchName,
                    InstructorName = $"{s.Instructor.User.FirstName} {s.Instructor.User.LastName}",
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    MaxCapacity = s.MaxCapacity,
                    CurrentEnrollments = s.Enrollments.Count(e => e.IsActive),
                    Status = s.Status
                })
                .ToListAsync();

            return View(sessions);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new SessionManagementViewModel
            {
                AvailableBranches = await _context.Branches
                    .Include(b => b.Center)
                    .Select(b => new BranchViewModel
                    {
                        BranchId = b.BranchId,
                        BranchName = b.BranchName,
                        CenterName = b.Center.CenterName
                    })
                    .ToListAsync(),

                AvailableInstructors = await _context.Instructors
                    .Include(i => i.User)
                    .Where(i => i.User.IsActive)
                    .Select(i => new InstructorViewModel
                    {
                        InstructorId = i.InstructorId,
                        FullName = $"{i.User.FirstName} {i.User.LastName}",
                        Specialization = i.Specialization
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload the lists
                model.AvailableBranches = await GetBranches();
                model.AvailableInstructors = await GetInstructors();
                return View(model);
            }

            // Validate time range
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time");
                model.AvailableBranches = await GetBranches();
                model.AvailableInstructors = await GetInstructors();
                return View(model);
            }

            // Check for instructor schedule conflicts
            var hasConflict = await _context.Sessions
                .Where(s => s.InstructorId == model.InstructorId &&
                           s.DayOfWeek == model.DayOfWeek &&
                           s.Status == SessionStatus.Active)
                .AnyAsync(s =>
                    (model.StartTime >= s.StartTime && model.StartTime < s.EndTime) ||
                    (model.EndTime > s.StartTime && model.EndTime <= s.EndTime) ||
                    (model.StartTime <= s.StartTime && model.EndTime >= s.EndTime));

            if (hasConflict)
            {
                ModelState.AddModelError("", "This instructor has a scheduling conflict during the selected time.");
                model.AvailableBranches = await GetBranches();
                model.AvailableInstructors = await GetInstructors();
                return View(model);
            }

            var session = new Session
            {
                SessionName = model.SessionName,
                BranchId = model.BranchId,
                InstructorId = model.InstructorId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                DayOfWeek = model.DayOfWeek,
                MaxCapacity = model.MaxCapacity,
                Status = SessionStatus.Active
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Session created successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<BranchViewModel>> GetBranches()
        {
            return await _context.Branches
                .Include(b => b.Center)
                .Select(b => new BranchViewModel
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    CenterName = b.Center.CenterName
                })
                .ToListAsync();
        }

        private async Task<List<InstructorViewModel>> GetInstructors()
        {
            return await _context.Instructors
                .Include(i => i.User)
                .Where(i => i.User.IsActive)
                .Select(i => new InstructorViewModel
                {
                    InstructorId = i.InstructorId,
                    FullName = $"{i.User.FirstName} {i.User.LastName}",
                    Specialization = i.Specialization
                })
                .ToListAsync();
        }
    }
}
