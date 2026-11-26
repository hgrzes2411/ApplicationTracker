using ApplicationTracker.Models;
using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.GhostDetection;

public class InterviewAndOfferRuleTests
{
    private static readonly DateOnly Date = new(2026, 1, 1);

    [Fact]
    public void InterviewRule_WhenInterviewExists_ReducesScoreByThirty()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Interview, Date)]);

        var result = new InterviewRule().Evaluate(GhostTestData.Context(application, Date));

        Assert.True(result.Triggered);
        Assert.Equal(-30, result.Score);
    }

    [Fact]
    public void InterviewRule_WhenInterviewDoesNotExist_DoesNotChangeScore()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Applied, Date)]);

        var result = new InterviewRule().Evaluate(GhostTestData.Context(application, Date));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
    }

    [Fact]
    public void OfferRule_WhenOfferExists_ReducesScoreByFifty()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Offer, Date)]);

        var result = new OfferRule().Evaluate(GhostTestData.Context(application, Date));

        Assert.True(result.Triggered);
        Assert.Equal(-50, result.Score);
    }

    [Fact]
    public void OfferRule_WhenOfferDoesNotExist_DoesNotChangeScore()
    {
        var application = GhostTestData.Application(
            history: [(ApplicationStatus.Applied, Date)]);

        var result = new OfferRule().Evaluate(GhostTestData.Context(application, Date));

        Assert.False(result.Triggered);
        Assert.Equal(0, result.Score);
    }
}
