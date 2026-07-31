using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
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

        return filtered;
    }
}