using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class CompanyAnalyticsCalculator
{
    private readonly GhostDetector _ghostDetector;

    public CompanyAnalyticsCalculator(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }


    public List<CompanyDashboardModel> Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return jobs
            .GroupBy(j => j.NormalizedCompany)
            .Select(group =>
            {
                var analyses = group
                    .Select(job =>
                        _ghostDetector.Analyze(job, jobs))
                    .ToList();


                return new CompanyDashboardModel
                {
                    Company = group.First().Company,

                    Applications = group.Count(),

                    AverageGhostScore = analyses.Any()
                        ? analyses.Average(a => a.Score)
                        : 0,

                    InterviewCount = group.Count(j =>
                        j.CurrentStatus == ApplicationStatus.Interview),

                    OfferCount = group.Count(j =>
                        j.CurrentStatus == ApplicationStatus.Offer)
                };
            })
            .OrderByDescending(c => c.AverageGhostScore)
            .ThenByDescending(c => c.Applications)
            .ToList();
    }
}