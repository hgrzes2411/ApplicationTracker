namespace ApplicationTracker.Models.Dashboard.Models;

public class SourceStatisticModel
{
    public string Source { get; set; } = "";

    public int Applications { get; set; }

    public int InterviewCount { get; set; }

    public int OfferCount { get; set; }

    public double SuccessRate =>
        Applications == 0
            ? 0
            : (double)OfferCount / Applications * 100;
}