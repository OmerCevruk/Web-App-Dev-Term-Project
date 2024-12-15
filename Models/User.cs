using System.ComponentModel.DataAnnotations;
using System;

namespace AthleteTracker.Models
{
    public enum UserRole
    {
        Admin,
        Instructor,
        Parent,
        Staff
    }

    public class User : BaseEntity
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

    }
}

