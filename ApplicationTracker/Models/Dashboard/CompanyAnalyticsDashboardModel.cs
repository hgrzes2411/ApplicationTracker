namespace ApplicationTracker.Models.Dashboard;

public class CompanyAnalyticsDashboardModel
{
    public List<CompanyDashboardModel> TopCompanies { get; set; } = [];

    public CompanyDashboardModel? MostSuspiciousCompany { get; set; }
}