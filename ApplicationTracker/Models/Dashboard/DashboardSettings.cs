namespace ApplicationTracker.Models.Dashboard;

public class DashboardSettings
{
    public bool ShowSummary { get; set; } = true;

    public bool ShowCharts { get; set; } = true;

    public bool ShowCompanyAnalytics { get; set; } = true;

    public bool ShowCompanyInsights { get; set; } = true;

    public bool ShowRecommendations { get; set; } = true;

    public bool ShowResponseAnalytics { get; set; } = true;

    public bool ShowSourceAnalytics { get; set; } = true;

    public bool ShowRecentActivity { get; set; } = true;

    public bool ShowSuccessRate { get; set; } = true;

    public bool ShowRecruitmentFunnel { get; set; } = true;
}