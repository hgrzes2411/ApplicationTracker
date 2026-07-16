namespace ApplicationTracker.Models
{
    public class StatusHistory
    {
        public ApplicationStatus Status { get; set; }

        public DateOnly Date { get; set; }

        public string? Note { get; set; }
    }
}
