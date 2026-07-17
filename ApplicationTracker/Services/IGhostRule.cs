namespace ApplicationTracker.Services;

public interface IGhostRule
{
    GhostRuleResult Evaluate(
        GhostAnalysisContext context);
}