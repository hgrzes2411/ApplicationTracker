using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;

namespace ApplicationTracker.Services.Dashboard;

public class SourceAnalyticsCalculator
{
    public SourceAnalyticsModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var sources = jobs
            .GroupBy(x => x.Source ?? "Unknown")
            .Select(group => new SourceStatisticModel
            {
                Source = group.Key,

                Applications = group.Count(),

                InterviewCount = group.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Interview),

                OfferCount = group.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Offer)
            })
            .OrderByDescending(x => x.OfferCount)
            .ThenByDescending(x => x.Applications)
            .ToList();


        return new SourceAnalyticsModel
        {
            Sources = sources
        };
    }
}