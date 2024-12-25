using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;

namespace AthleteTracker.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var branches = await _context.Branches
                    .Include(b => b.Center)
                    .OrderBy(b => b.Center.CenterName)
                    .ThenBy(b => b.BranchName)
                    .Select(b => new
                    {
                        b.BranchId,
                        b.BranchName,
                        CenterName = b.Center.CenterName,
                        b.Address,
                        b.Phone,
                        b.Email,
                        SessionCount = b.Sessions.Count(s => s.Status == SessionStatus.Active)
                    })
                    .ToListAsync();

                if (!branches.Any())
                {
                    ViewBag.Message = "No branches found.";
                }

                return View(branches);
            }
            catch (Exception ex)
            {
                // Add this temporarily for debugging
                return Content($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new BranchCreateViewModel
            {
                AvailableCenters = await _context.SportsCenters
                    .OrderBy(sc => sc.CenterName)
                    .Select(sc => new SportsCenterViewModel
                    {
                        CenterId = sc.CenterId,
                        CenterName = sc.CenterName
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCenters = await GetSportsCenters();
                return View(model);
            }

            var branch = new Branch
            {
                BranchName = model.BranchName,
                CenterId = model.CenterId,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email
            };

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Branch created successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SportsCenterViewModel>> GetSportsCenters()
        {
            return await _context.SportsCenters
                .OrderBy(sc => sc.CenterName)
                .Select(sc => new SportsCenterViewModel
                {
                    CenterId = sc.CenterId,
                    CenterName = sc.CenterName
                })
                .ToListAsync();
        }
    }
}
