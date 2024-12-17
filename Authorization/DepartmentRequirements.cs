using Microsoft.AspNetCore.Authorization;

namespace AthleteTracker.Authorization
{
    public class DepartmentRequirement : IAuthorizationRequirement
    {
        public string[] AllowedDepartments { get; }

        public DepartmentRequirement(params string[] allowedDepartments)
        {
            AllowedDepartments = allowedDepartments;
        }
    }
}

