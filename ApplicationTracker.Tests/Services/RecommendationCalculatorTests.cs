using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.Dashboard.Calculators;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.Services.GhostDetection.Rules;

namespace ApplicationTracker.Tests.Services;

public class RecommendationCalculatorTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);
    private readonly RecommendationCalculator _calculator = new(new GhostDetector(
    [
        new CompanyHistoryRule(),
        new NoResponseRule(),
        new RepeatedPostingRule()
    ]));

    [Fact]
    public void Calculate_WhenScoreIsBelow40_ReturnsInfoRecommendation()
    {
        var job = JobWithNoResponse(Today.AddDays(-29));

        var result = _calculator.Calculate([job]);

        var item = Assert.Single(result.Items);
        Assert.Equal(RecommendationLevel.Info, item.Level);
        Assert.Equal("Proces wygląda prawidłowo.", item.Message);
    }

    [Fact]
    public void Calculate_WhenScoreIsBetween40And69_ReturnsWarningRecommendation()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var jobs = Enumerable.Range(1, 4)
            .Select(index => JobWithNoResponse(today.AddDays(-29), "Acme", $"job-{index}"))
            .ToArray();

        jobs = [.. jobs, JobWithNoResponse(today.AddDays(-29), "Acme", "job-5")];

        var result = _calculator.Calculate(jobs);

        Assert.NotEmpty(result.Items);
        var item = result.Items[0];
        Assert.Equal(40, item.GhostScore);
        Assert.Equal(RecommendationLevel.Warning, item.Level);
    }

    [Fact]
    public void Calculate_WhenScoreIsAtLeast70_ReturnsCriticalRecommendation()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var jobs = Enumerable.Range(1, 5)
            .Select(index => JobWithNoResponse(today.AddDays(-60), "Acme", $"job-{index}"))
            .ToArray();

        var result = _calculator.Calculate(jobs);

        Assert.NotEmpty(result.Items);
        var item = result.Items[0];
        Assert.Equal(70, item.GhostScore);
        Assert.Equal(RecommendationLevel.Critical, item.Level);
    }

    [Fact]
    public void Calculate_OrdersItemsByGhostScoreDescending()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var low = JobWithNoResponse(today.AddDays(-29), "low");
        var high = JobWithNoResponse(today.AddDays(-60), "high");

        var result = _calculator.Calculate([low, high]);

        Assert.Equal("high", result.Items[0].Company);
        Assert.True(result.Items[0].GhostScore > result.Items[1].GhostScore);
    }

    private static JobApplication JobWithNoResponse(
        DateOnly date,
        string company = "Acme",
        string? externalJobId = null) => new()
    {
        Id = Guid.NewGuid(),
        Company = company,
        Position = "Developer",
        ExternalJobId = externalJobId ?? company,
        History = [new StatusHistory { Status = ApplicationStatus.Applied, Date = date }]
    };
}
