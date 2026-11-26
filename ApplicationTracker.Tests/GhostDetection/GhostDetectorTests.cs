using ApplicationTracker.Models.Analysis;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Tests.GhostDetection;

public class GhostDetectorTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);

    [Theory]
    [InlineData(0, GhostRiskLevel.Low, "Niskie ryzyko ghost joba.")]
    [InlineData(39, GhostRiskLevel.Low, "Niskie ryzyko ghost joba.")]
    [InlineData(40, GhostRiskLevel.Medium, "Średnie ryzyko ghost joba.")]
    [InlineData(59, GhostRiskLevel.Medium, "Średnie ryzyko ghost joba.")]
    [InlineData(60, GhostRiskLevel.High, "Wysokie ryzyko ghost joba.")]
    public void Analyze_WhenRulesProduceScore_AssignsExpectedRisk(
        int score,
        GhostRiskLevel expectedRisk,
        string expectedSummary)
    {
        var application = GhostTestData.Application();
        var detector = new GhostDetector([new FixedRule(score)]);

        var result = detector.Analyze(application, [application]);

        Assert.Equal(score, result.Score);
        Assert.Equal(expectedRisk, result.RiskLevel);
        Assert.Equal(expectedSummary, result.Summary);
        Assert.Single(result.Rules);
        Assert.Equal(score >= 60, result.IsGhost);
    }

    [Fact]
    public void Analyze_SumsAllRuleScoresAndCountsCompanyApplications()
    {
        var application = GhostTestData.Application(company: "Acme");
        var sameCompany = GhostTestData.Application(company: " acme ");
        var otherCompany = GhostTestData.Application(company: "Other");
        var detector = new GhostDetector([new FixedRule(30), new FixedRule(20)]);

        var result = detector.Analyze(application, [application, sameCompany, otherCompany]);

        Assert.Equal(50, result.Score);
        Assert.Equal(2, result.Rules.Count);
        Assert.Equal(2, result.ApplicationsToCompany);
    }

    private sealed class FixedRule(int score) : IGhostRule
    {
        public GhostRuleResult Evaluate(GhostAnalysisContext context) => new()
        {
            Name = "Fixed rule",
            Score = score,
            Triggered = score > 0,
            Description = $"Score: {score}"
        };
    }
}
