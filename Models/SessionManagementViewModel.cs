using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class SessionManagementViewModel
    {
        [Required(ErrorMessage = "Session name is required")]
        [Display(Name = "Session Name")]
        [StringLength(100)]
        public string SessionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Instructor is required")]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Day of week is required")]
        [Display(Name = "Day of Week")]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "Maximum capacity is required")]
        [Display(Name = "Maximum Capacity")]
        [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100")]
        public int MaxCapacity { get; set; }

        public List<BranchViewModel> AvailableBranches { get; set; } = new();
        public List<InstructorViewModel> AvailableInstructors { get; set; } = new();
    }

    public class BranchViewModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string CenterName { get; set; } = string.Empty;
    }

    public class InstructorViewModel
    {
        public int InstructorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}
