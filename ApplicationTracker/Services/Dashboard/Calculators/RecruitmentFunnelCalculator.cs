using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard.Models;

namespace ApplicationTracker.Services.Dashboard.Calculators;

public class RecruitmentFunnelCalculator
{
    public RecruitmentFunnelModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return new RecruitmentFunnelModel
        {
            Applications = jobs.Count,

            Responses = jobs.Count(job =>
                job.History.Any(h =>
                    h.Status != ApplicationStatus.Applied)),

            Interviews = jobs.Count(job =>
                job.CurrentStatus == ApplicationStatus.Interview
                || job.CurrentStatus == ApplicationStatus.Offer),

            Offers = jobs.Count(job =>
                job.CurrentStatus == ApplicationStatus.Offer)
        };
    }
}