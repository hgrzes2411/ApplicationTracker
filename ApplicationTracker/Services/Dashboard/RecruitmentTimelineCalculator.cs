using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;

namespace ApplicationTracker.Services.Dashboard;

public class RecruitmentTimelineCalculator
{
    public RecruitmentTimelineModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var events = jobs
            .SelectMany(job =>
                job.History.Select(history =>
                    new TimelineEventModel
                    {
                        Company = job.Company,
                        Status = history.Status,
                        Date = history.Date,
                        Note = history.Note
                    }))
            .OrderByDescending(x => x.Date)
            .ToList();

        return new RecruitmentTimelineModel
        {
            Events = events
        };
    }
}