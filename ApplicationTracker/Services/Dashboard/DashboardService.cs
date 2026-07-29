using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.Jobs;

namespace ApplicationTracker.Services.Dashboard;

public class DashboardService
{
    private readonly JobService _jobService;
    private readonly DashboardSummaryCalculator _summaryCalculator;
    private readonly CompanyAnalyticsCalculator _companyAnalyticsCalculator;
    private readonly RecommendationCalculator _recommendationCalculator;
    private readonly ResponseAnalyticsCalculator _responseAnalyticsCalculator;
    private readonly RecentActivityCalculator _recentActivityCalculator;
    private readonly StatusChartCalculator _statusChartCalculator;
    private readonly SuccessRateCalculator _successRateCalculator;
    private readonly RecruitmentFunnelCalculator _recruitmentFunnelCalculator;
    private readonly RecruitmentTimelineCalculator _recruitmentTimelineCalculator;
    private readonly SourceAnalyticsCalculator _sourceAnalyticsCalculator;
    private readonly CompanyInsightsCalculator _companyInsightsCalculator;

    public DashboardService(
        JobService jobService,
        DashboardSummaryCalculator summaryCalculator,
        CompanyAnalyticsCalculator companyAnalyticsCalculator,
        RecommendationCalculator recommendationCalculator,
        ResponseAnalyticsCalculator responseAnalyticsCalculator,
        RecentActivityCalculator recentActivityCalculator,
        StatusChartCalculator statusChartCalculator,
        SuccessRateCalculator successRateCalculator,
        RecruitmentFunnelCalculator recruitmentFunnelCalculator,
        RecruitmentTimelineCalculator recruitmentTimelineCalculator,
        SourceAnalyticsCalculator sourceAnalyticsCalculator,
        CompanyInsightsCalculator companyInsightsCalculator)
    {
        _jobService = jobService;
        _summaryCalculator = summaryCalculator;
        _companyAnalyticsCalculator = companyAnalyticsCalculator;
        _recommendationCalculator = recommendationCalculator;
        _responseAnalyticsCalculator = responseAnalyticsCalculator;
        _recentActivityCalculator = recentActivityCalculator;
        _statusChartCalculator = statusChartCalculator;
        _successRateCalculator = successRateCalculator;
        _recruitmentFunnelCalculator = recruitmentFunnelCalculator;
        _recruitmentTimelineCalculator = recruitmentTimelineCalculator;
        _sourceAnalyticsCalculator = sourceAnalyticsCalculator;
        _companyInsightsCalculator = companyInsightsCalculator;
    }

    [Obsolete]
    public async Task<DashboardModel> GetDashboardAsync()
    {
        var jobs = await _jobService.GetAllAsync();

        var companies = _companyAnalyticsCalculator.Calculate(jobs);

        var successRate = _successRateCalculator.Calculate(jobs);

        var recruitmentFunnel = _recruitmentFunnelCalculator.Calculate(jobs);

        var timeline = _recruitmentTimelineCalculator.Calculate(jobs);

        var sourceAnalytics =
    _sourceAnalyticsCalculator.Calculate(jobs);
        var companyInsights =
    _companyInsightsCalculator.Calculate(jobs);

        var now = DateTime.Now;

        var welcomeMessage =
            jobs.Any(j => j.Status == ApplicationStatus.Offer)
                ? "Świetna robota! Masz aktywne oferty pracy."
                : jobs.Any(j => j.Status == ApplicationStatus.Interview)
                    ? "Masz aktywne procesy rekrutacyjne."
                    : "Powodzenia w kolejnych aplikacjach!";

        return new DashboardModel
        {
            Summary = _summaryCalculator.Calculate(jobs),

            CompanyAnalytics = new CompanyAnalyticsDashboardModel
            {
                TopCompanies = companies,
                MostSuspiciousCompany = companies.FirstOrDefault()
            },

            Recommendations = _recommendationCalculator.Calculate(jobs),

            ResponseAnalytics = _responseAnalyticsCalculator.Calculate(jobs),

            RecentActivity = _recentActivityCalculator.Calculate(jobs),

            StatusChart = _statusChartCalculator.Calculate(jobs),

            SuccessRate = successRate,

            RecruitmentFunnel = recruitmentFunnel,

            Timeline = timeline,

            SourceAnalytics = sourceAnalytics,
            CompanyInsights = companyInsights,
            LastUpdated = now,

            WelcomeMessage = welcomeMessage,
        };
    }
}