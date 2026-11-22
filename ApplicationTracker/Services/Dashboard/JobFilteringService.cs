using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using ApplicationTracker.Models.Sorting;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class JobFilteringService
{
    private readonly GhostDetector _ghostDetector;

    public JobFilteringService(GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }

    public IEnumerable<JobApplication> FilterJobs(IEnumerable<JobApplication> jobs, JobFilter filter)
    {
        var allJobs = jobs.ToList();
        var filtered = allJobs.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            filtered = filtered.Where(job =>
                job.Company.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase) ||
                job.Position.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.Status.HasValue)
        {
            filtered = filtered.Where(job => job.CurrentStatus == filter.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Company))
        {
            filtered = filtered.Where(job => job.Company.Equals(filter.Company, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Source))
        {
            filtered = filtered.Where(job => job.Source?.Equals(filter.Source, StringComparison.OrdinalIgnoreCase) == true);
        }

        if (filter.WorkMode.HasValue)
        {
            filtered = filtered.Where(job => job.WorkMode == filter.WorkMode.Value);
        }

        if (filter.MinGhostScore.HasValue || filter.MaxGhostScore.HasValue)
        {
            filtered = filtered.Where(job =>
            {
                var score = _ghostDetector.Analyze(job, allJobs).Score;
                return (!filter.MinGhostScore.HasValue || score >= filter.MinGhostScore.Value) &&
                       (!filter.MaxGhostScore.HasValue || score <= filter.MaxGhostScore.Value);
            });
        }

        return filter.SortOption switch
        {
            JobSortOption.ApplicationDateAscending => filtered.OrderBy(job => job.ApplicationDate),
            JobSortOption.ApplicationDateDescending => filtered.OrderByDescending(job => job.ApplicationDate),
            JobSortOption.CompanyAscending => filtered.OrderBy(job => job.Company),
            JobSortOption.CompanyDescending => filtered.OrderByDescending(job => job.Company),
            JobSortOption.PositionAscending => filtered.OrderBy(job => job.Position),
            JobSortOption.PositionDescending => filtered.OrderByDescending(job => job.Position),
            JobSortOption.StatusAscending => filtered.OrderBy(job => job.CurrentStatus),
            JobSortOption.StatusDescending => filtered.OrderByDescending(job => job.CurrentStatus),
            JobSortOption.GhostScoreAscending => filtered.OrderBy(job => _ghostDetector.Analyze(job, allJobs).Score),
            JobSortOption.GhostScoreDescending => filtered.OrderByDescending(job => _ghostDetector.Analyze(job, allJobs).Score),
            _ => filtered.OrderByDescending(job => job.ApplicationDate)
        };
    }
}
