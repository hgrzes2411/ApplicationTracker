using ApplicationTracker.Models;
using ApplicationTracker.Services.Export;

namespace ApplicationTracker.Tests.Export;

public class CsvExportServiceTests
{
    private readonly CsvExportService _service = new();

    [Fact]
    public void GenerateCsv_WhenJobsAreEmpty_ReturnsHeaderOnly()
    {
        var result = _service.GenerateCsv([]);

        Assert.Equal("Company,Position,Status,Source,WorkMode,ApplicationDate", result);
    }

    [Fact]
    public void GenerateCsv_ExportsStatusAndDateInExpectedFormat()
    {
        var job = new JobApplication
        {
            Company = "Acme",
            Position = "Developer",
            Source = "LinkedIn",
            WorkMode = WorkMode.Remote,
            ApplicationDate = new DateOnly(2026, 2, 3),
            History = [new StatusHistory
            {
                Status = ApplicationStatus.Interview,
                Date = new DateOnly(2026, 2, 4)
            }]
        };

        var result = _service.GenerateCsv([job]);

        Assert.Contains("\"Acme\",\"Developer\",\"Interview\",\"LinkedIn\",\"Remote\",\"2026-02-03\"", result);
    }

    [Fact]
    public void GenerateCsv_EscapesQuotesInFieldValues()
    {
        var job = new JobApplication
        {
            Company = "Acme \"Polska\"",
            Position = "Developer",
            ApplicationDate = new DateOnly(2026, 2, 3)
        };

        var result = _service.GenerateCsv([job]);

        Assert.Contains("\"Acme \"\"Polska\"\"\"", result);
    }
}
