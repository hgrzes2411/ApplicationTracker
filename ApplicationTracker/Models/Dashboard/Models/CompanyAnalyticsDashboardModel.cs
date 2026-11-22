namespace ApplicationTracker.Models.Dashboard.Models;

public class CompanyAnalyticsDashboardModel
{
    public List<CompanyDashboardModel> TopCompanies { get; set; } = [];

    public CompanyDashboardModel? MostSuspiciousCompany { get; set; }
}