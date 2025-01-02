using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AthleteTracker.Data;
using AthleteTracker.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class SportsCenterController : Controller
{
    private readonly ApplicationDbContext _context;

    public SportsCenterController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var centers = await _context.SportsCenters
            .Select(c => new
            {
                c.CenterId,
                c.CenterName,
                c.Address,
                c.Phone,
                c.Email,
                BranchCount = c.Branches.Count
            })
            .ToListAsync();

        return View(centers);
    }

    // TODO:
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SportsCenterCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var center = new SportsCenter
        {
            CenterName = model.CenterName,
            Address = model.Address,
            Phone = model.Phone,
            Email = model.Email
        };

        _context.SportsCenters.Add(center);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Sports Center created successfully.";
        return RedirectToAction(nameof(Index));
    }
}
