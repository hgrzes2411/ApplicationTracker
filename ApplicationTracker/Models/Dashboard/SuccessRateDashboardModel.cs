namespace ApplicationTracker.Models.Dashboard;

public class SuccessRateDashboardModel
{
    public int TotalApplications { get; set; }

    public int Responses { get; set; }

    public int Interviews { get; set; }

    public int Offers { get; set; }

    public double ResponseRate { get; set; }

    public double InterviewRate { get; set; }

    public double OfferRate { get; set; }

    public double SuccessRate => OfferRate;

    public int SuccessfulApplications => Offers;
}