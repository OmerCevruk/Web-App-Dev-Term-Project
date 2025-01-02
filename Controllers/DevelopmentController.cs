using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;
using AthleteTracker.Services;

namespace AthleteTracker.Controllers
{


    [Authorize]
    public class DevelopmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentService _studentService;

        public DevelopmentController(
            ApplicationDbContext context,
            IStudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        [Authorize(Roles = "Instructor,Admin,Parent")]
        public async Task<IActionResult> Index(int? studentId)
        {
            if (!studentId.HasValue)
            {
                var selectionViewModel = await _studentService.GetStudentSelectionList(
                    "Development",
                    "Index",
                    "View Development Records");

                return View("~/Views/Shared/SelectStudent.cshtml", selectionViewModel);
            }

            var student = await _context.Students
                .Include(s => s.Parent)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return NotFound();

            var records = await _context.DevelopmentRecords
                .Where(r => r.StudentId == studentId)
                .OrderBy(r => r.RecordDate)
                .ToListAsync();

            var viewModel = new DevelopmentRecordViewModel
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                ParentName = $"{student.Parent.User.FirstName} {student.Parent.User.LastName}",
                DateOfBirth = student.DateOfBirth,
                Records = records,
                HeightData = string.Join(",", records.Select(r => r.Height)),
                WeightData = string.Join(",", records.Select(r => r.Weight)),
                DateLabels = string.Join(",", records.Select(r => r.RecordDate.ToString("MM/dd/yyyy")))
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Create(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound();

            ViewBag.StudentName = $"{student.FirstName} {student.LastName}";
            return View(new DevelopmentRecordInputModel { StudentId = studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Create(DevelopmentRecordInputModel model)
        {
            if (ModelState.IsValid)
            {
                var record = new DevelopmentRecord
                {
                    StudentId = model.StudentId,
                    RecordDate = model.RecordDate.ToUniversalTime(),
                    Height = model.Height,
                    Weight = model.Weight,
                    InstructorComments = model.InstructorComments
                };

                _context.DevelopmentRecords.Add(record);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { studentId = model.StudentId });
            }

            var student = await _context.Students.FindAsync(model.StudentId);
            ViewBag.StudentName = $"{student?.FirstName} {student?.LastName}";
            return View(model);
        }
    }
}
