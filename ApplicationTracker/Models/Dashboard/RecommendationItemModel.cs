namespace ApplicationTracker.Models.Dashboard;

public class RecommendationItemModel
{
    public string Company { get; set; } = string.Empty;

    public int GhostScore { get; set; }

    public string Message { get; set; } = string.Empty;

    public RecommendationLevel Level { get; set; }
}