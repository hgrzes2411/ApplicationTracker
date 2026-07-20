using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;

namespace ApplicationTracker.Services.Dashboard;

public class RecentActivityCalculator
{
    public List<ActivityDashboardModel> Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return jobs
            .SelectMany(job => job.History.Select(history =>
                new ActivityDashboardModel
                {
                    Company = job.Company,
                    Status = history.Status,
                    Date = history.Date,
                    Note = history.Note
                }))
            .OrderByDescending(x => x.Date)
            .Take(10)
            .ToList();
    }
}