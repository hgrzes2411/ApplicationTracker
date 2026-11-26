using ApplicationTracker.Models;

namespace ApplicationTracker.Tests.Models;

public class JobApplicationTests
{
    [Fact]
    public void CurrentStatus_WhenHistoryIsEmpty_ReturnsApplied()
    {
        var application = new JobApplication();

        Assert.Equal(ApplicationStatus.Applied, application.CurrentStatus);
    }

    [Fact]
    public void CurrentStatus_WhenHistoryIsUnordered_ReturnsStatusFromLatestDate()
    {
        var application = new JobApplication
        {
            History =
            [
                new StatusHistory { Status = ApplicationStatus.Interview, Date = new DateOnly(2026, 1, 10) },
                new StatusHistory { Status = ApplicationStatus.Applied, Date = new DateOnly(2026, 1, 1) },
                new StatusHistory { Status = ApplicationStatus.Offer, Date = new DateOnly(2026, 1, 20) }
            ]
        };

        Assert.Equal(ApplicationStatus.Offer, application.CurrentStatus);
    }

    [Fact]
    public void NormalizedCompany_TrimsWhitespaceAndUsesLowercase()
    {
        var application = new JobApplication { Company = "  ACME Corporation  " };

        Assert.Equal("acme corporation", application.NormalizedCompany);
    }

    [Fact]
    public void SalaryRange_WhenBothValuesExist_ReturnsInvariantRange()
    {
        var application = new JobApplication
        {
            SalaryFrom = 10000.5m,
            SalaryTo = 15000m
        };

        Assert.Equal("10000.5–15000", application.SalaryRange);
    }

    [Fact]
    public void SalaryRange_WhenOnlyOneValueExists_ReturnsThatValue()
    {
        var from = new JobApplication { SalaryFrom = 10000m };
        var to = new JobApplication { SalaryTo = 15000.25m };

        Assert.Equal("10000", from.SalaryRange);
        Assert.Equal("15000.25", to.SalaryRange);
    }

    [Fact]
    public void SalaryRange_WhenNoValueExists_ReturnsNull()
    {
        var application = new JobApplication();

        Assert.Null(application.SalaryRange);
    }

    [Fact]
    public void UpdateFrom_CopiesEditableFieldsAndHistory()
    {
        var originalId = Guid.NewGuid();
        var original = new JobApplication
        {
            Id = originalId,
            Company = "Old Company",
            Position = "Old Position",
            Notes = "Old notes"
        };
        var history = new List<StatusHistory>
        {
            new() { Status = ApplicationStatus.Interview, Date = new DateOnly(2026, 1, 1) }
        };
        var updated = new JobApplication
        {
            Id = Guid.NewGuid(),
            Company = "New Company",
            Position = "New Position",
            Notes = "New notes",
            History = history
        };

        original.UpdateFrom(updated);

        Assert.Equal(originalId, original.Id);
        Assert.Equal("New Company", original.Company);
        Assert.Equal("New Position", original.Position);
        Assert.Equal("New notes", original.Notes);
        Assert.Same(history, original.History);
    }
}
