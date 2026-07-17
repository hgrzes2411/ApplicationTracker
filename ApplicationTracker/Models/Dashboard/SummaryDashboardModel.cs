namespace ApplicationTracker.Models.Dashboard;

public class SummaryDashboardModel
{
    public int TotalApplications { get; set; }

    public int ActiveApplications { get; set; }

    public int InterviewCount { get; set; }

    public int OfferCount { get; set; }

    public int RejectedCount { get; set; }

    public int GhostApplications { get; set; }
}