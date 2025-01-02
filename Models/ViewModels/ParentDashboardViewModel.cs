using System;
using System.Collections.Generic;

namespace AthleteTracker.Models
{
    public class ParentDashboardViewModel
    {
        public List<StudentViewModel> Students { get; set; } = new();
        public List<PaymentViewModel> UpcomingPayments { get; set; } = new();
    }

    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<EnrollmentViewModel> Enrollments { get; set; } = new();
    }
}
