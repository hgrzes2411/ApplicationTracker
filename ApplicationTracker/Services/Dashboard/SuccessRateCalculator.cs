using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;

namespace ApplicationTracker.Services.Dashboard;

public class SuccessRateCalculator
{
    public SuccessRateDashboardModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var totalApplications = jobs.Count;

        var responses = jobs.Count(job =>
            job.History.Any(history =>
                history.Status != ApplicationStatus.Applied));

        var interviews = jobs.Count(job =>
            job.CurrentStatus == ApplicationStatus.Interview ||
            job.History.Any(history =>
                history.Status == ApplicationStatus.Interview));

        var offers = jobs.Count(job =>
            job.CurrentStatus == ApplicationStatus.Offer ||
            job.History.Any(history =>
                history.Status == ApplicationStatus.Offer));


        return new SuccessRateDashboardModel
        {
            TotalApplications = totalApplications,

            Responses = responses,

            Interviews = interviews,

            Offers = offers,

            ResponseRate = CalculatePercentage(
                responses,
                totalApplications),

            InterviewRate = CalculatePercentage(
                interviews,
                totalApplications),

            OfferRate = CalculatePercentage(
                offers,
                totalApplications)
        };
    }


    private double CalculatePercentage(
        int value,
        int total)
    {
        if (total == 0)
        {
            return 0;
        }

        return Math.Round(
            (double)value / total * 100,
            1);
    }
}