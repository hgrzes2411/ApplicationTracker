using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.GhostDetection;

public class RepeatedPostingRuleTests
{
    private readonly RepeatedPostingRule _rule = new();
    private static readonly DateOnly Date = new(2026, 1, 1);

    [Fact]
    public void Evaluate_WhenExternalJobIdIsMissing_DoesNotTrigger()
    {
        var application = GhostTestData.Application(externalJobId: null);

        var result = _rule.Evaluate(GhostTestData.Context(application, Date));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
        Assert.Equal("Brak identyfikatora ogłoszenia.", result.Description);
    }

    [Fact]
    public void Evaluate_WhenJobIdOccursOnce_DoesNotTrigger()
    {
        var application = GhostTestData.Application(externalJobId: "job-1");

        var result = _rule.Evaluate(GhostTestData.Context(application, Date));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
    }

    [Fact]
    public void Evaluate_WhenJobIdOccursMoreThanOnce_AddsTenPoints()
    {
        var applications = new[]
        {
            GhostTestData.Application(externalJobId: "job-1"),
            GhostTestData.Application(externalJobId: "job-1"),
            GhostTestData.Application(externalJobId: "other-job")
        };

        var result = _rule.Evaluate(GhostTestData.Context(applications[0], Date, applications));

        Assert.True(result.Triggered);
        Assert.Equal(10, result.Score);
        Assert.Contains("2 razy", result.Description);
    }
}
