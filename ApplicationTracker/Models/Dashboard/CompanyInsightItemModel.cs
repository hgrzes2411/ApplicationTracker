using ApplicationTracker.Models;

namespace ApplicationTracker.Models.Dashboard;

public class CompanyInsightItemModel
{
    public string Company { get; set; } = "";

    public int Applications { get; set; }

    public int InterviewCount { get; set; }

    public int OfferCount { get; set; }

    public double AverageGhostScore { get; set; }

    public double SuccessRate { get; set; }

    public double AverageResponseTime { get; set; }

    public DateOnly? FirstApplication { get; set; }

    public DateOnly? LastApplication { get; set; }
}