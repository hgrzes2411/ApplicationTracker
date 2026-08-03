using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using ApplicationTracker.Models.Sorting;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Jobs;

public class JobFilteringService
{
    private readonly GhostDetector _ghostDetector;

    public JobFilteringService(GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }

    public IEnumerable<JobApplication> FilterJobs(
        IEnumerable<JobApplication> jobs,
        JobFilter filter)
    {
        var filtered = jobs;

        // ==========================
        // Wyszukiwanie tekstowe
        // ==========================

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            filtered = filtered.Where(j =>
                j.Company.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase)
                ||
                j.Position.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase));
        }

        // ==========================
        // Status
        // ==========================

        if (filter.Status.HasValue)
        {
            filtered = filtered.Where(j =>
                j.CurrentStatus == filter.Status.Value);
        }

        // ==========================
        // Firma
        // ==========================

        if (!string.IsNullOrWhiteSpace(filter.Company))
        {
            filtered = filtered.Where(j =>
                j.Company.Equals(
                    filter.Company,
                    StringComparison.OrdinalIgnoreCase));
        }

        // ==========================
        // Źródło
        // ==========================

        if (!string.IsNullOrWhiteSpace(filter.Source))
        {
            filtered = filtered.Where(j =>
                j.Source.Equals(
                    filter.Source,
                    StringComparison.OrdinalIgnoreCase));
        }

        // ==========================
        // Tryb pracy
        // ==========================

        if (filter.WorkMode.HasValue)
        {
            filtered = filtered.Where(j =>
                j.WorkMode == filter.WorkMode.Value);
        }

        // ==========================
        // Ghost Score
        // ==========================

        if (filter.MinGhostScore.HasValue ||
            filter.MaxGhostScore.HasValue)
        {
            filtered = filtered.Where(job =>
            {
                var score = _ghostDetector
                    .Analyze(job, jobs)
                    .Score;

                if (filter.MinGhostScore.HasValue &&
                    score < filter.MinGhostScore.Value)
                    return false;

                if (filter.MaxGhostScore.HasValue &&
                    score > filter.MaxGhostScore.Value)
                    return false;

                return true;
            });
        }

        // ==========================
        // Sortowanie
        // ==========================

        filtered = SortJobs(filtered, filter.SortOption, jobs);

        return filtered;
    }

    private IEnumerable<JobApplication> SortJobs(
        IEnumerable<JobApplication> jobs,
        JobSortOption option,
        IEnumerable<JobApplication> allJobs)
    {
        return option switch
        {
            JobSortOption.ApplicationDateAscending =>
                jobs.OrderBy(j => j.ApplicationDate),

            JobSortOption.ApplicationDateDescending =>
                jobs.OrderByDescending(j => j.ApplicationDate),

            JobSortOption.CompanyAscending =>
                jobs.OrderBy(j => j.Company),

            JobSortOption.CompanyDescending =>
                jobs.OrderByDescending(j => j.Company),

            JobSortOption.PositionAscending =>
                jobs.OrderBy(j => j.Position),

            JobSortOption.PositionDescending =>
                jobs.OrderByDescending(j => j.Position),

            JobSortOption.StatusAscending =>
                jobs.OrderBy(j => j.CurrentStatus),

            JobSortOption.StatusDescending =>
                jobs.OrderByDescending(j => j.CurrentStatus),

            JobSortOption.GhostScoreAscending =>
                jobs.OrderBy(j =>
                    _ghostDetector
                        .Analyze(j, allJobs)
                        .Score),

            JobSortOption.GhostScoreDescending =>
                jobs.OrderByDescending(j =>
                    _ghostDetector
                        .Analyze(j, allJobs)
                        .Score),

            _ =>
                jobs.OrderByDescending(j => j.ApplicationDate)
        };
    }
}