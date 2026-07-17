namespace ApplicationTracker.Models.Statistics;

public class DashboardStatistics
{
    public int TotalApplications { get; set; }

    public int ActiveApplications { get; set; }

    public int GhostApplications { get; set; }

    public int Interviews { get; set; }

    public int Offers { get; set; }

    public int Rejected { get; set; }

    public double AverageResponseDays { get; set; }

    public List<CompanyStatistics> Companies { get; set; } = [];
}