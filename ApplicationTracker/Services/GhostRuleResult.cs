namespace ApplicationTracker.Services;

public class GhostRuleResult
{
    public string Name { get; set; } = "";

    public bool Triggered { get; set; }

    public int Score { get; set; }

    public string Description { get; set; } = "";
}