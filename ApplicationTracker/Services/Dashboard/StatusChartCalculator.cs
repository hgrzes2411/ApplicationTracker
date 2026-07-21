using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard.Charts;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class StatusChartCalculator
{
    private readonly GhostDetector _ghostDetector;

    public StatusChartCalculator(GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }

    public StatusChartModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return new StatusChartModel
        {
            Applied = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Applied),

            Interview = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Interview),

            Offer = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Offer),

            Rejected = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Rejected),

            Ghost = jobs.Count(job =>
                _ghostDetector.Analyze(job, jobs).IsGhost)
        };
    }
}