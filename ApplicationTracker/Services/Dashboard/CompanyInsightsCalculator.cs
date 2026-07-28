using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class CompanyInsightsCalculator
{
    private readonly GhostDetector _ghostDetector;

    public CompanyInsightsCalculator(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }

    public CompanyInsightsModel Calculate(
    IReadOnlyCollection<JobApplication> jobs)
    {
        var companies = jobs
            .GroupBy(j => j.NormalizedCompany)
            .Select(group =>
            {
                var companyJobs = group.ToList();

                var analyses = companyJobs
                    .Select(job => _ghostDetector.Analyze(job, jobs))
                    .ToList();

                var responseTimes = companyJobs
                    .Select(job =>
                    {
                        var history = job.History
                            .OrderBy(x => x.Date)
                            .ToList();

                        var applied = history.FirstOrDefault(x =>
                            x.Status == ApplicationStatus.Applied);

                        var response = history.FirstOrDefault(x =>
                            x.Status != ApplicationStatus.Applied);

                        if (applied == null || response == null)
                            return (int?)null;

                        return response.Date.DayNumber - applied.Date.DayNumber;
                    })
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToList();

                return new CompanyInsightItemModel
                {
                    Company = companyJobs.First().Company,

                    Applications = companyJobs.Count,

                    InterviewCount = companyJobs.Count(x =>
                        x.CurrentStatus == ApplicationStatus.Interview),

                    OfferCount = companyJobs.Count(x =>
                        x.CurrentStatus == ApplicationStatus.Offer),

                    AverageGhostScore = analyses.Any()
                        ? analyses.Average(x => x.Score)
                        : 0,

                    SuccessRate = companyJobs.Count == 0
                        ? 0
                        : (double)companyJobs.Count(x =>
                            x.CurrentStatus == ApplicationStatus.Offer)
                            / companyJobs.Count * 100,

                    AverageResponseTime = responseTimes.Any()
                        ? responseTimes.Average()
                        : 0,

                    FirstApplication = companyJobs.Min(x => x.ApplicationDate),

                    LastApplication = companyJobs.Max(x => x.ApplicationDate)
                };
            })
            .OrderByDescending(x => x.Applications)
            .ToList();

        return new CompanyInsightsModel
        {
            Companies = companies
        };
    }
}