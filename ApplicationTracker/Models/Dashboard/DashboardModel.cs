namespace ApplicationTracker.Models.Dashboard;

public class DashboardModel
{
    public SummaryDashboardModel Summary { get; set; } = new();

    public CompanyAnalyticsDashboardModel CompanyAnalytics { get; set; } = new();

    public List<ActivityDashboardModel> RecentActivity { get; set; } = [];

    public RecommendationDashboardModel Recommendations { get; set; } = new();
}