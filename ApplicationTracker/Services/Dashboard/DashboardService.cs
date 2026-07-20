using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.Services.Jobs;

namespace ApplicationTracker.Services.Dashboard;

public class DashboardService
{
    private readonly JobService _jobService;
    private readonly GhostDetector _ghostDetector;
    private readonly DashboardSummaryCalculator _summaryCalculator;
    private readonly CompanyAnalyticsCalculator _companyAnalyticsCalculator;
    private readonly RecommendationCalculator _recommendationCalculator;
    private readonly ResponseAnalyticsCalculator _responseAnalyticsCalculator;
    private readonly RecentActivityCalculator _recentActivityCalculator;
    public DashboardService(
    JobService jobService,
    GhostDetector ghostDetector,
    DashboardSummaryCalculator summaryCalculator,
    CompanyAnalyticsCalculator companyAnalyticsCalculator,
    RecommendationCalculator recommendationCalculator,
    ResponseAnalyticsCalculator responseAnalyticsCalculator,
    RecentActivityCalculator recentActivityCalculator)
    {
        _jobService = jobService;
        _ghostDetector = ghostDetector;
        _summaryCalculator = summaryCalculator;
        _companyAnalyticsCalculator = companyAnalyticsCalculator;
        _recommendationCalculator = recommendationCalculator;
        _responseAnalyticsCalculator = responseAnalyticsCalculator;
        _recentActivityCalculator = recentActivityCalculator;
    }

    public async Task<DashboardModel> GetDashboardAsync()
    {
        var jobs = await _jobService.GetAllAsync();

        var analyses = jobs
            .Select(job => _ghostDetector.Analyze(job, jobs))
            .ToList();

        var recommendations =
    _recommendationCalculator.Calculate(jobs);

        var companies = _companyAnalyticsCalculator.Calculate(jobs);

        var responseAnalytics =
    _responseAnalyticsCalculator.Calculate(jobs);

        var recentActivity =
     _recentActivityCalculator.Calculate(jobs);

        return new DashboardModel
        {
            
                Summary = _summaryCalculator.Calculate(jobs),
            

            CompanyAnalytics = new CompanyAnalyticsDashboardModel
            {
                TopCompanies = companies,

                MostSuspiciousCompany = companies.FirstOrDefault()
            },

            Recommendations = recommendations,

            ResponseAnalytics = responseAnalytics,

            RecentActivity = recentActivity



        };

    }
}