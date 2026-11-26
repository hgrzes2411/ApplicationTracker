using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.GhostDetection;

public class CompanyHistoryRuleTests
{
    private readonly CompanyHistoryRule _rule = new();
    private static readonly DateOnly Date = new(2026, 1, 1);

    [Theory]
    [InlineData(2, 0)]
    [InlineData(3, 20)]
    [InlineData(4, 30)]
    [InlineData(5, 40)]
    [InlineData(6, 40)]
    public void Evaluate_WhenCompanyHasDistinctRecruitmentCycles_ReturnsExpectedScore(
        int cycleCount,
        int expectedScore)
    {
        var applications = Enumerable.Range(1, cycleCount)
            .Select(index => GhostTestData.Application(externalJobId: $"job-{index}"))
            .ToArray();

        var result = _rule.Evaluate(GhostTestData.Context(applications[0], Date, applications));

        Assert.Equal(expectedScore, result.Score);
        Assert.Equal(expectedScore > 0, result.Triggered);
    }

    [Fact]
    public void Evaluate_WhenExternalJobIdsRepeat_CountsOneRecruitmentCycle()
    {
        var applications = new[]
        {
            GhostTestData.Application(externalJobId: "same-job"),
            GhostTestData.Application(externalJobId: "same-job"),
            GhostTestData.Application(externalJobId: "job-2")
        };

        var result = _rule.Evaluate(GhostTestData.Context(applications[0], Date, applications));

        Assert.Equal(0, result.Score);
        Assert.Contains("2 cykle", result.Description);
    }

    [Fact]
    public void Evaluate_WhenCompanyNameDiffersOnlyByCaseAndWhitespace_UsesSameCompany()
    {
        var applications = new[]
        {
            GhostTestData.Application(company: " Acme ", externalJobId: "job-1"),
            GhostTestData.Application(company: "ACME", externalJobId: "job-2"),
            GhostTestData.Application(company: "acme", externalJobId: "job-3")
        };

        var result = _rule.Evaluate(GhostTestData.Context(applications[0], Date, applications));

        Assert.Equal(20, result.Score);
        Assert.True(result.Triggered);
    }

    [Fact]
    public void Evaluate_WhenApplicationBelongsToAnotherCompany_IgnoresIt()
    {
        var applications = new[]
        {
            GhostTestData.Application(company: "Acme", externalJobId: "job-1"),
            GhostTestData.Application(company: "Other", externalJobId: "job-2"),
            GhostTestData.Application(company: "Other", externalJobId: "job-3"),
            GhostTestData.Application(company: "Other", externalJobId: "job-4")
        };

        var result = _rule.Evaluate(GhostTestData.Context(applications[0], Date, applications));

        Assert.Equal(0, result.Score);
    }
}
