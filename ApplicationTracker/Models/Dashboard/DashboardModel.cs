using ApplicationTracker.Models.Dashboard.Charts;

namespace ApplicationTracker.Models.Dashboard;

public class DashboardModel
{
    public SummaryDashboardModel Summary { get; set; } = new();

    public CompanyAnalyticsDashboardModel CompanyAnalytics { get; set; } = new();

    public List<ActivityDashboardModel> RecentActivity { get; set; } = [];

    public RecommendationDashboardModel Recommendations { get; set; } = new();

    public ResponseAnalyticsDashboardModel ResponseAnalytics { get; set; } = new();

    public StatusChartModel StatusChart { get; set; } = new();

    public SuccessRateDashboardModel SuccessRate { get; set; } = new();

    public RecruitmentFunnelModel RecruitmentFunnel { get; set; } = new();

    public RecruitmentTimelineModel Timeline { get; set; } = new();

    public SourceAnalyticsModel SourceAnalytics { get; set; } = new();

    public CompanyInsightsModel CompanyInsights { get; set; } = new();

    public DateTime LastUpdated { get; set; }

    public string WelcomeMessage { get; set; } = string.Empty;
}