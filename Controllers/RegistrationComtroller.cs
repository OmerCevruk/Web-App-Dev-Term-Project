using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegistrationController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult RegisterParent()
        {
            return View();
        }

        // user submits parent login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterParent(ParentRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(model);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // create user
                    var user = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Role = UserRole.Parent,
                        IsActive = true
                    };

                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // create parent
                    var parent = new Parent
                    {
                        UserId = user.UserId,
                        EmergencyContact = model.EmergencyContact,
                        EmergencyPhone = model.EmergencyPhone
                    };

                    _context.Parents.Add(parent);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return RedirectToAction("Login", "Account");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                }
            }

            // if model is not valid
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        // user submits student login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parent = await _context.Parents
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.User.Email == model.ParentEmail);

                if (parent == null)
                {
                    ModelState.AddModelError("ParentEmail", "Parent email not found. Parent must register first.");
                    return View(model);
                }

                var student = new Student
                {
                    ParentId = parent.ParentId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    MedicalConditions = model.MedicalConditions,
                    IsActive = true
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return RedirectToAction("RegisterStudentSuccess");
            }

            return View(model);
        }

        public IActionResult RegisterStudentSuccess()
        {
            return View();
        }
    }
}
