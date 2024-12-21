using Microsoft.AspNetCore.Authorization;
using AthleteTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace AthleteTracker.Authorization
{
    public class DepartmentHandler : IAuthorizationHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirement in context.Requirements.OfType<DepartmentRequirement>())
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                {
                    continue;
                }

                // Check if user is admin
                if (!user.IsInRole("Admin"))
                {
                    continue;
                }

                // Get user ID from claims
                var userIdClaim = user.FindFirst("UserId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    continue;
                }

                // Get admin's department
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.UserId == userId);

                if (admin != null && requirement.AllowedDepartments.Contains(admin.Department))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
