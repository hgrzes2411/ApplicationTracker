using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.Services.Jobs;

namespace ApplicationTracker.Services.Dashboard;

public class DashboardService
{
    private readonly JobService _jobService;
    private readonly GhostDetector _ghostDetector;

    public DashboardService(
        JobService jobService,
        GhostDetector ghostDetector)
    {
        _jobService = jobService;
        _ghostDetector = ghostDetector;
    }

    public async Task<DashboardModel> GetDashboardAsync()
    {
        var jobs = await _jobService.GetAllAsync();

        var analyses = jobs
            .Select(job => _ghostDetector.Analyze(job, jobs))
            .ToList();

        var recommendations = CalculateRecommendations(jobs);

        var companies = CalculateCompanies(jobs);

        var activity = CalculateRecentActivity(jobs);

        var responseAnalytics = CalculateResponseAnalytics(jobs);

        var recentActivity = CalculateRecentActivity(jobs);

        return new DashboardModel
        {
            Summary = new SummaryDashboardModel
            {
                TotalApplications = jobs.Count,

                ActiveApplications = jobs.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Applied),

                InterviewCount = jobs.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Interview),

                OfferCount = jobs.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Offer),

                RejectedCount = jobs.Count(x =>
                    x.CurrentStatus == ApplicationStatus.Rejected),

                GhostApplications = analyses.Count(x => x.IsGhost)
            },

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

    private List<CompanyDashboardModel> CalculateCompanies(
        IReadOnlyCollection<JobApplication> jobs)
    {
        return jobs
            .GroupBy(j => j.NormalizedCompany)
            .Select(group =>
            {
                var analyses = group
                    .Select(job => _ghostDetector.Analyze(job, jobs))
                    .ToList();

                return new CompanyDashboardModel
                {
                    Company = group.First().Company,

                    Applications = group.Count(),

                    AverageGhostScore = analyses.Any()
                        ? analyses.Average(a => a.Score)
                        : 0,

                    InterviewCount = group.Count(j =>
                        j.CurrentStatus == ApplicationStatus.Interview),

                    OfferCount = group.Count(j =>
                        j.CurrentStatus == ApplicationStatus.Offer)
                };
            })
            .OrderByDescending(c => c.AverageGhostScore)
            .ThenByDescending(c => c.Applications)
            .ToList();
    }

    private List<ActivityDashboardModel> CalculateRecentActivity(
    IReadOnlyCollection<JobApplication> jobs)
    {
        return jobs
            .SelectMany(job => job.History.Select(history =>
                new ActivityDashboardModel
                {
                    Company = job.Company,
                    Status = history.Status,
                    Date = history.Date,
                    Note = history.Note
                }))
            .OrderByDescending(x => x.Date)
            .Take(10)
            .ToList();
    }

    private RecommendationDashboardModel CalculateRecommendations(
     IReadOnlyCollection<JobApplication> jobs)
    {
        var items = jobs
            .Select(job =>
            {
                var analysis = _ghostDetector.Analyze(job, jobs);

                var recommendation = new RecommendationItemModel
                {
                    Company = job.Company,
                    GhostScore = analysis.Score
                };

                if (analysis.Score >= 70)
                {
                    recommendation.Level = RecommendationLevel.Critical;

                    recommendation.Message =
                        "Wysokie ryzyko ghostingu. Rozważ zakończenie procesu.";
                }
                else if (analysis.Score >= 40)
                {
                    recommendation.Level = RecommendationLevel.Warning;

                    recommendation.Message =
                        "Proces wymaga obserwacji. Brak wyraźnych oznak zakończenia.";
                }
                else
                {
                    recommendation.Level = RecommendationLevel.Info;

                    recommendation.Message =
                        "Proces wygląda prawidłowo.";
                }

                return recommendation;
            })
            .OrderByDescending(x => x.GhostScore)
            .ToList();


        return new RecommendationDashboardModel
        {
            Items = items
        };
    }

    private ResponseAnalyticsDashboardModel CalculateResponseAnalytics(
    IReadOnlyCollection<JobApplication> jobs)
    {
        var responseTimes = jobs
            .Select(job =>
            {
                var history = job.History
                    .OrderBy(x => x.Date)
                    .ToList();

                var appliedStatus = history
                    .FirstOrDefault(x =>
                        x.Status == ApplicationStatus.Applied);

                var firstResponse = history
                    .FirstOrDefault(x =>
                        x.Status != ApplicationStatus.Applied);

                if (appliedStatus == null || firstResponse == null)
                {
                    return (int?)null;
                }

                return firstResponse.Date.DayNumber
                       - appliedStatus.Date.DayNumber;
            })
            .Where(x => x.HasValue)
            .Select(x => x.Value)
            .ToList();


        return new ResponseAnalyticsDashboardModel
        {
            AverageResponseTime = responseTimes.Any()
                ? responseTimes.Average()
                : 0,

            FastestResponseTime = responseTimes.Any()
                ? responseTimes.Min()
                : 0,

            SlowestResponseTime = responseTimes.Any()
                ? responseTimes.Max()
                : 0
        };
    }


}