namespace ScrumStandUpTrackerProject.DTOs
{
    public class DailyStatusDTO
    {

        public int Id { get; set; }
        public int DeveloperId { get; set; }
        public string? DeveloperName { get; set; }
        public string TaskDetails { get; set; }
        public string DidYesterday { get; set; }
        public string DoingToday { get; set; }
        public string Blockers { get; set; }
        public DateTime? SubmissionDate { get; set; }
    }
}