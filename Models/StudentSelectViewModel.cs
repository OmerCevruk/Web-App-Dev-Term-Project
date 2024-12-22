using System.ComponentModel.DataAnnotations;

namespace AthleteTracker.Models
{
    public class StudentSelectViewModel
    {
        public int StudentId { get; set; }

        [Display(Name = "Student Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Parent Name")]
        public string ParentName { get; set; } = string.Empty;
    }
}
