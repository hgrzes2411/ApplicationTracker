using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class DashboardSummaryCalculator
{
    private readonly GhostDetector _ghostDetector;

    public DashboardSummaryCalculator(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }


    public SummaryDashboardModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var analyses = jobs
            .Select(job =>
                _ghostDetector.Analyze(job, jobs))
            .ToList();


        return new SummaryDashboardModel
        {
            TotalApplications = jobs.Count,

            ActiveApplications = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Applied),

            InterviewCount = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Interview),

            OfferCount = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Offer),

            RejectedCount = jobs.Count(x =>
                x.CurrentStatus == ApplicationStatus.Rejected),

            GhostApplications = analyses.Count(x =>
                x.IsGhost)
        };
    }
}