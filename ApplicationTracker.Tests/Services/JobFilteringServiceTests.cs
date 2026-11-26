using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using ApplicationTracker.Models.Sorting;
using ApplicationTracker.Services.Dashboard;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.Services;

public class JobFilteringServiceTests
{
    private static readonly DateOnly Date = new(2026, 1, 1);
    private readonly JobFilteringService _service = new(new GhostDetector(
    [
        new CompanyHistoryRule(),
        new NoResponseRule(),
        new RepeatedPostingRule()
    ]));

    [Fact]
    public void FilterJobs_WhenSearchTextMatchesCompanyOrPosition_IsCaseInsensitive()
    {
        var jobs = new[]
        {
            Job("Acme", "Backend Developer"),
            Job("Other", "Designer")
        };

        var result = _service.FilterJobs(jobs, new JobFilter { SearchText = "DEVELOPER" }).ToList();

        Assert.Single(result);
        Assert.Equal("Acme", result[0].Company);
    }

    [Fact]
    public void FilterJobs_WhenMultipleCriteriaAreSet_ReturnsOnlyMatchingJobs()
    {
        var jobs = new[]
        {
            Job("Acme", "Developer", ApplicationStatus.Interview, "LinkedIn", WorkMode.Remote),
            Job("Acme", "Developer", ApplicationStatus.Applied, "LinkedIn", WorkMode.Remote),
            Job("Other", "Developer", ApplicationStatus.Interview, "Indeed", WorkMode.Remote)
        };

        var result = _service.FilterJobs(jobs, new JobFilter
        {
            Company = "acme",
            Source = "linkedin",
            Status = ApplicationStatus.Interview,
            WorkMode = WorkMode.Remote
        }).ToList();

        Assert.Single(result);
        Assert.Equal("Acme", result[0].Company);
    }

    [Fact]
    public void FilterJobs_WhenSortingByApplicationDateDescending_ReturnsNewestFirst()
    {
        var oldest = Job("Acme", "Old", applicationDate: new DateOnly(2026, 1, 1));
        var newest = Job("Acme", "New", applicationDate: new DateOnly(2026, 2, 1));

        var result = _service.FilterJobs(
            [oldest, newest],
            new JobFilter { SortOption = JobSortOption.ApplicationDateDescending }).ToList();

        Assert.Equal([newest, oldest], result);
    }

    private static JobApplication Job(
        string company,
        string position,
        ApplicationStatus status = ApplicationStatus.Applied,
        string source = "Source",
        WorkMode workMode = WorkMode.Remote,
        DateOnly? applicationDate = null)
    {
        return new JobApplication
        {
            Id = Guid.NewGuid(),
            Company = company,
            Position = position,
            Source = source,
            WorkMode = workMode,
            ApplicationDate = applicationDate ?? Date,
            History = [new StatusHistory { Status = status, Date = Date }]
        };
    }
}
