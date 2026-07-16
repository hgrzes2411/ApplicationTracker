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

        [Obsolete("Removed. Use History and CurrentStatus instead.")]
        public ApplicationStatus Status { get; set; }

        public string? Source { get; set; }

        public List<StatusHistory> History { get; set; } = [];

        public ApplicationStatus CurrentStatus =>
     History.Any()
         ? History.MaxBy(x => x.Date)!.Status
         : ApplicationStatus.Applied;

        public string? Url { get; set; }

        public string? Notes { get; set; }

        public void UpdateFrom(JobApplication other)
        {
            Company = other.Company;

            Position = other.Position;

            Location = other.Location;

            WorkMode = other.WorkMode;

            SalaryFrom = other.SalaryFrom;

            SalaryTo = other.SalaryTo;

            ApplicationDate = other.ApplicationDate;

            Url = other.Url;

            Notes = other.Notes;

            History = other.History;
        }

    }
}
