using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AthleteTracker.Models
{
    public class ReportViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-1);

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int NewEnrollments { get; set; }
        public int Withdrawals { get; set; }
        public decimal TotalIncome { get; set; }
        public List<SessionStatistics> SessionStats { get; set; } = new();
    }

    public class SessionStatistics
    {
        public string SessionName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int CurrentEnrollments { get; set; }
        public int MaxCapacity { get; set; }
        public decimal OccupancyRate => MaxCapacity > 0 ?
            (decimal)CurrentEnrollments / MaxCapacity * 100 : 0;
    }
}
