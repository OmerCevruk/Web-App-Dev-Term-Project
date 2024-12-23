using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class EnrollmentViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Session is required")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(1);

        // to displaying available sessions
        public List<SessionViewModel> AvailableSessions { get; set; } = new List<SessionViewModel>();
    }

    public class SessionViewModel
    {
        public int SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public int CurrentEnrollments { get; set; }
        public int MaxCapacity { get; set; }
    }
}
