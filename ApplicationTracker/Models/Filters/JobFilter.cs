using ApplicationTracker.Models;
using ApplicationTracker.Models.Sorting;
namespace ApplicationTracker.Models.Filters
{

    public class JobFilter
    {
        public string SearchText { get; set; } = "";

        public ApplicationStatus? Status { get; set; }

        public string? Company { get; set; }

        public string? Source { get; set; }

        public WorkMode? WorkMode { get; set; }

        public int? MinGhostScore { get; set; }

        public int? MaxGhostScore { get; set; }

        public JobSortOption SortOption { get; set; }
    = JobSortOption.ApplicationDateDescending;
    }
}