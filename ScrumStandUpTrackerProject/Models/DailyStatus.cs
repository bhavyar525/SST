using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumStandUpTrackerProject.Models
{
    public class DailyStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Developer")]
        public int DeveloperId { get; set; }
        [Required]
        public Developer Developer { get; set; }  // Navigation property

        [Required]
        public string DeveloperName { get; set; }
        [Required]
        public string TaskDetails { get; set; }
        [Required]
        public string DidYesterday { get; set; }
        [Required]
        public string DoingToday { get; set; }
        [Required]
        public string Blockers { get; set; }
        [Required]

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;  // Default to current time
    }
}