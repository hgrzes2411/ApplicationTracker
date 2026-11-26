using ApplicationTracker.Models;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Tests.GhostDetection;

internal static class GhostTestData
{
    public static JobApplication Application(
        string company = "Acme",
        string? externalJobId = "job-1",
        params (ApplicationStatus Status, DateOnly Date)[] history)
    {
        return new JobApplication
        {
            Id = Guid.NewGuid(),
            Company = company,
            Position = "Developer",
            ExternalJobId = externalJobId,
            History = history.Select(item => new StatusHistory
            {
                Status = item.Status,
                Date = item.Date
            }).ToList()
        };
    }

    public static GhostAnalysisContext Context(
        JobApplication application,
        DateOnly today,
        params JobApplication[] allApplications)
    {
        return new GhostAnalysisContext
        {
            Application = application,
            AllApplications = allApplications.Length == 0 ? [application] : allApplications,
            Today = today
        };
    }
}
