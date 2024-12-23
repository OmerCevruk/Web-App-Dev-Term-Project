using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Services
{
    public interface IStudentService
    {
        Task<StudentSelectionViewModel> GetStudentSelectionList(string targetController, string targetAction, string actionText);
        Task<Student?> GetStudentById(int id);
    }

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentSelectionViewModel> GetStudentSelectionList(
            string targetController,
            string targetAction,
            string actionText)
        {
            var students = await _context.Students
                .Include(s => s.Parent)
                .ThenInclude(p => p.User)
                .Where(s => s.IsActive)
                .Select(s => new StudentListItemViewModel
                {
                    StudentId = s.StudentId,
                    FullName = $"{s.FirstName} {s.LastName}",
                    ParentName = $"{s.Parent.User.FirstName} {s.Parent.User.LastName}"
                })
                .ToListAsync();

            return new StudentSelectionViewModel
            {
                Students = students,
                TargetController = targetController,
                TargetAction = targetAction,
                ActionText = actionText
            };
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students
                .Include(s => s.Parent)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }
    }
}
