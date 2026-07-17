using ApplicationTracker.Models;

namespace ApplicationTracker.Services;

public class GhostAnalysisContext
{
    public required JobApplication Application { get; init; }

    public required IReadOnlyCollection<JobApplication> AllApplications { get; init; }

    public DateOnly Today { get; init; } =
        DateOnly.FromDateTime(DateTime.Today);
}