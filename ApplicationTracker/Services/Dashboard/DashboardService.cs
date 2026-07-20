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

        var companies = CalculateCompanies(jobs);

        var activity = CalculateRecentActivity(jobs);

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

            TopCompanies = companies,

            MostSuspiciousCompany = companies.FirstOrDefault(),

            RecentActivity = activity

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
            .SelectMany(job =>
                job.History.Select(history =>
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
}