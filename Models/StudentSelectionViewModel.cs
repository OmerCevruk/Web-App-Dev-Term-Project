namespace AthleteTracker.Models
{
    public class StudentSelectionViewModel
    {
        public List<StudentListItemViewModel> Students { get; set; } = new();
        public string TargetController { get; set; } = string.Empty;
        public string TargetAction { get; set; } = string.Empty;
        public string ActionText { get; set; } = "Select";

        public string GetActionUrl(int studentId)
        {
            return $"/{TargetController}/{TargetAction}?studentId={studentId}";
        }
    }

    public class StudentListItemViewModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
    }
}
