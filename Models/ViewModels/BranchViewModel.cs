namespace AthleteTracker.Models
{
    public class BranchViewModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string CenterName { get; set; } = string.Empty;
        public int CenterId { get; set; }
        public string FullDisplayName => $"{BranchName} ({CenterName})";
    }
}
