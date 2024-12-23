namespace AthleteTracker.Models
{
    public class SessionIndexViewModel
    {
        public int SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentEnrollments { get; set; }
        public SessionStatus Status { get; set; }
    }
}
