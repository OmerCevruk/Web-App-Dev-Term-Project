using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class ParentDashboardViewModel
    {
        public List<Student> Students { get; set; } = new();
        public List<Payment> UpcomingPayments { get; set; } = new();
    }
}
