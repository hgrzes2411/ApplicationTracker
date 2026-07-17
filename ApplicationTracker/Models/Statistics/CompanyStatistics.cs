namespace ApplicationTracker.Models.Statistics;

public class CompanyStatistics
{
    public string Company { get; set; } = "";

    public int Applications { get; set; }

    public int GhostScore { get; set; }

    public int Interviews { get; set; }

    public int Offers { get; set; }
}