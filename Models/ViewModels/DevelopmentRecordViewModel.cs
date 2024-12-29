namespace AthleteTracker.Models
{
    public class DevelopmentRecordViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public List<DevelopmentRecord> Records { get; set; } = new();

        public string HeightData { get; set; } = string.Empty;
        public string WeightData { get; set; } = string.Empty;
        public string DateLabels { get; set; } = string.Empty;
    }

}
