using Microsoft.AspNetCore.Authorization;
using AthleteTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace AthleteTracker.Authorization
{
    public class DepartmentHandler : AuthorizationHandler<DepartmentRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DepartmentRequirement requirement)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return;
            }

            // Check if user is admin
            var isAdmin = user.IsInRole("Admin");
            if (!isAdmin)
            {
                return;
            }

            // Get user ID from claims
            var userIdClaim = user.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return;
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
