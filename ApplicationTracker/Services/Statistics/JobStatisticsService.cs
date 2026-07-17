using ApplicationTracker.Models;
using ApplicationTracker.Models.Statistics;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Statistics;

public class JobStatisticsService
{
    private readonly GhostDetector _ghostDetector;

    public JobStatisticsService(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }

    public DashboardStatistics Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return new DashboardStatistics
        {
            TotalApplications = jobs.Count
        };
    }
}