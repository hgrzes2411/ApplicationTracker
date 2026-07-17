namespace ApplicationTracker.Models.Dashboard;

public class CompanyDashboardModel
{
    public string Company { get; set; } = "";

    public int Applications { get; set; }

    public double AverageGhostScore { get; set; }

    public int InterviewCount { get; set; }

    public int OfferCount { get; set; }
}