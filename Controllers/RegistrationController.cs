using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AthleteTracker.Data;
using AthleteTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace AthleteTracker.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public RegistrationController(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        // Parent Controller
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

        // Student Controller
        [HttpGet]
        [Authorize(Policy = "HR")]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        // user submits student login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HR")]
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
                    DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
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

        // Instructor Controller
        [HttpGet]
        [Authorize(Policy = "HR")]
        public async Task<IActionResult> RegisterInstructor()
        {
            try
            {
                var viewModel = new InstructorRegistrationViewModel
                {
                    AvailableBranches = await _context.Branches
                        .Include(b => b.Center)
                        .Select(b => new BranchViewModel
                        {
                            BranchId = b.BranchId,
                            BranchName = b.BranchName,
                            CenterName = b.Center.CenterName
                        })
                        .ToListAsync()
                };

                // Initialize other properties to avoid null reference
                viewModel.FirstName = string.Empty;
                viewModel.LastName = string.Empty;
                viewModel.Email = string.Empty;
                viewModel.Phone = string.Empty;
                viewModel.Password = string.Empty;
                viewModel.ConfirmPassword = string.Empty;
                viewModel.Specialization = string.Empty;
                viewModel.SelectedBranchIds = new List<int>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in RegisterInstructor: {ex}");
                // Return a view with empty model for graceful degradation
                return View(new InstructorRegistrationViewModel
                {
                    AvailableBranches = new List<BranchViewModel>()
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HR")]
        public async Task<IActionResult> RegisterInstructor(InstructorRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    model.AvailableBranches = await GetBranchesAsync();
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
                        Role = UserRole.Instructor,
                        IsActive = true
                    };

                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // create instructor
                    var instructor = new Instructor
                    {
                        UserId = user.UserId,
                        Specialization = model.Specialization,
                        HireDate = DateTime.SpecifyKind(model.HireDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc)
                    };

                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();

                    // create instructor-branch relationships
                    foreach (var branchId in model.SelectedBranchIds)
                    {
                        var instructorBranch = new InstructorBranch
                        {
                            InstructorId = instructor.InstructorId,
                            BranchId = branchId
                        };
                        _context.InstructorBranches.Add(instructorBranch);
                    }

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

            model.AvailableBranches = await GetBranchesAsync();
            return View(model);
        }

        // Admin Controller
        [HttpGet]
        [Authorize(Policy = "IT")]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IT")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationViewModel model)
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
                    // Create user
                    var user = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Role = UserRole.Admin,
                        IsActive = true
                    };

                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Create admin profile
                    var admin = new Admin
                    {
                        UserId = user.UserId,
                        Department = model.Department
                    };

                    _context.Admins.Add(admin);
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

            return View(model);
        }

        private async Task<List<BranchViewModel>> GetBranchesAsync()
        {
            return await _context.Branches
                .Include(b => b.Center)
                .OrderBy(b => b.Center.CenterName)
                .ThenBy(b => b.BranchName)
                .Select(b => new BranchViewModel
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    CenterId = b.CenterId,
                    CenterName = b.Center.CenterName
                })
                .ToListAsync();
        }
    }
}
