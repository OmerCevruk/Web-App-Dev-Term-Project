using System.ComponentModel.DataAnnotations;

public class DevelopmentRecordInputModel
{
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Record Date")]
    [DataType(DataType.Date)]
    public DateTime RecordDate { get; set; } = DateTime.Today;

    [Required]
    [Range(0, 250)]
    [Display(Name = "Height (cm)")]
    public decimal Height { get; set; }

    [Required]
    [Range(0, 200)]
    [Display(Name = "Weight (kg)")]
    public decimal Weight { get; set; }

    [Display(Name = "Coach Comments")]
    public string? InstructorComments { get; set; }
}
