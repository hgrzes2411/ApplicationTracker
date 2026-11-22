namespace ApplicationTracker.Services.GhostDetection;

public interface IGhostRule
{
    GhostRuleResult Evaluate(GhostAnalysisContext context);
}
