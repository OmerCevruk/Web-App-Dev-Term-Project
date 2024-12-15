//this class is  used for all the tables with dates
//postgresql expects dates to be UTC
namespace AthleteTracker.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
