using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;

namespace ApplicationTracker.Services.Dashboard;

public class ResponseAnalyticsCalculator
{
    public ResponseAnalyticsDashboardModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var responseTimes = jobs
            .Select(job =>
            {
                var history = job.History
                    .OrderBy(x => x.Date)
                    .ToList();

                var appliedStatus = history
                    .FirstOrDefault(x =>
                        x.Status == ApplicationStatus.Applied);

                var firstResponse = history
                    .FirstOrDefault(x =>
                        x.Status != ApplicationStatus.Applied);

                if (appliedStatus == null || firstResponse == null)
                {
                    return (int?)null;
                }

                return firstResponse.Date.DayNumber
                       - appliedStatus.Date.DayNumber;
            })
            .Where(x => x.HasValue)
            .Select(x => x.Value)
            .ToList();


        return new ResponseAnalyticsDashboardModel
        {
            AverageResponseTime = responseTimes.Any()
                ? responseTimes.Average()
                : 0,

            FastestResponseTime = responseTimes.Any()
                ? responseTimes.Min()
                : 0,

            SlowestResponseTime = responseTimes.Any()
                ? responseTimes.Max()
                : 0
        };
    }
}