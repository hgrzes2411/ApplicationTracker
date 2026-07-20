namespace ApplicationTracker.Models.Dashboard;

public class DashboardModel
{
    public SummaryDashboardModel Summary { get; set; } = new();

    public List<CompanyDashboardModel> TopCompanies { get; set; } = [];

    public CompanyDashboardModel? MostSuspiciousCompany { get; set; }

    public List<ActivityDashboardModel> RecentActivity { get; set; } = [];
}