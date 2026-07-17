using ApplicationTracker.Models;
using ApplicationTracker.Models.Analysis;

namespace ApplicationTracker.Services.GhostDetection;

public class GhostDetector
{
    private readonly IEnumerable<IGhostRule> _rules;


    public GhostDetector(IEnumerable<IGhostRule> rules)
    {
        _rules = rules;
    }


    public GhostAnalysisResult Analyze(
        JobApplication application,
        IEnumerable<JobApplication> allApplications)
    {
        var context = new GhostAnalysisContext
        {
            Application = application,
            AllApplications = allApplications.ToList()
        };


        var result = new GhostAnalysisResult();


        foreach (var rule in _rules)
        {
            var ruleResult = rule.Evaluate(context);

            result.Rules.Add(ruleResult);
            result.Score += ruleResult.Score;
        }


        result.ApplicationsToCompany =
            context.AllApplications.Count(x =>
                x.NormalizedCompany ==
                application.NormalizedCompany);


        result.RiskLevel = result.Score switch
        {
            >= 60 => GhostRiskLevel.High,
            >= 40 => GhostRiskLevel.Medium,
            _ => GhostRiskLevel.Low
        };
        result.Summary = BuildSummary(result);


        return result;
    }

    private static string BuildSummary(GhostAnalysisResult result)
    {
        return result.RiskLevel switch
        {
            GhostRiskLevel.Low =>
                "Niskie ryzyko ghost joba.",

            GhostRiskLevel.Medium =>
                "Średnie ryzyko ghost joba.",

            GhostRiskLevel.High =>
                "Wysokie ryzyko ghost joba.",

            _ => ""
        };
    }
}