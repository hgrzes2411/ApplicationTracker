namespace ApplicationTracker.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }

        public string Company { get; set; } = "";

        public string Position { get; set; } = "";

        public string? Location { get; set; }

        public WorkMode WorkMode { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }

        public DateOnly ApplicationDate { get; set; }

        public ApplicationStatus Status { get; set; }

        public string? Url { get; set; }

        public string? Notes { get; set; }
    }
}
