using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Pole Firma jest wymagane")]
        public string Company { get; set; } = "";

        public string NormalizedCompany =>
        Company.Trim().ToLower();

        [Required(ErrorMessage = "Pole Stanowisko jest wymagane")]
        public string Position { get; set; } = "";

        [Required(ErrorMessage = "Pole Lokalizacja jest wymagane")]
        public string? Location { get; set; }

        public WorkMode WorkMode { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }

        public string? SalaryRange
        {
            get
            {
                if (SalaryFrom is null && SalaryTo is null)
                {
                    return field;
                }

                var from = SalaryFrom?.ToString("0.##", CultureInfo.InvariantCulture);
                var to = SalaryTo?.ToString("0.##", CultureInfo.InvariantCulture);

                return from is not null && to is not null
                    ? $"{from}–{to}"
                    : from ?? to;
            }
            set => field = value;
        }

        public DateOnly ApplicationDate { get; set; }

        [Obsolete("Removed. Use History and CurrentStatus instead.")]
        public ApplicationStatus Status { get; set; }

        public string? Source { get; set; }

        public string? ExternalJobId { get; set; }

        public List<StatusHistory> History { get; set; } = [];

        public ApplicationStatus CurrentStatus =>
            History.Count > 0
                ? History.OrderBy(x => x.Date).Last().Status
                : ApplicationStatus.Applied;

        [Required(ErrorMessage = "Pole Link jest wymagane")]
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

            SalaryRange = other.SalaryRange;

            ApplicationDate = other.ApplicationDate;

            Url = other.Url;

            Notes = other.Notes;

            History = other.History;
        }

    }
}
