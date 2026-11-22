namespace ApplicationTracker.Models.Dashboard.Models;

public class TimelineEventModel
{
    public string Company { get; set; } = "";

    public ApplicationStatus Status { get; set; }

    public DateOnly Date { get; set; }

    public string? Note { get; set; }
}