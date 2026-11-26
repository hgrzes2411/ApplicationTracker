using ApplicationTracker.Models;
using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.GhostDetection;

public class NoResponseRuleTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);
    private readonly NoResponseRule _rule = new();

    [Fact]
    public void Evaluate_WhenHistoryIsEmpty_ReturnsNoScore()
    {
        var application = GhostTestData.Application(history: []);

        var result = _rule.Evaluate(GhostTestData.Context(application, Today));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
        Assert.Equal("Brak historii statusów.", result.Description);
    }

    [Theory]
    [InlineData(29, 0)]
    [InlineData(30, 20)]
    [InlineData(59, 20)]
    [InlineData(60, 30)]
    public void Evaluate_WhenLatestStatusIsApplied_UsesExpectedBoundaryScore(int days, int expectedScore)
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Applied, Today.AddDays(-days))]);

        var result = _rule.Evaluate(GhostTestData.Context(application, Today));

        Assert.Equal(expectedScore, result.Score);
        Assert.Equal(expectedScore > 0, result.Triggered);
    }

    [Fact]
    public void Evaluate_WhenLatestStatusIsNotApplied_DoesNotTrigger()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Applied, Today.AddDays(-90)),
                      (ApplicationStatus.Interview, Today.AddDays(-1))]);

        var result = _rule.Evaluate(GhostTestData.Context(application, Today));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
        Assert.Equal("Firma wykonała akcję w procesie.", result.Description);
    }

    [Fact]
    public void Evaluate_WhenHistoryIsUnordered_UsesTheLatestStatus()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Interview, Today.AddDays(-1)),
                      (ApplicationStatus.Applied, Today.AddDays(-60))]);

        var result = _rule.Evaluate(GhostTestData.Context(application, Today));

        Assert.Equal(0, result.Score);
    }
}
