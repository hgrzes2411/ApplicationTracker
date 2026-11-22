namespace ApplicationTracker.Models
{
    public class StatusHistory
    {
        public Guid Id { get; set; }

        public Guid JobApplicationId { get; set; }

        public JobApplication JobApplication { get; set; } = null!;

        public ApplicationStatus Status { get; set; }

        public DateOnly Date { get; set; }

        public string? Note { get; set; }
    }
}
