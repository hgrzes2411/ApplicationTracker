using ApplicationTracker.Models.Analysis;

namespace ApplicationTracker.Services.GhostDetection.Rules;

public class RepeatedPostingRule : IGhostRule
{
    public GhostRuleResult Evaluate(
        GhostAnalysisContext context)
    {
        var result = new GhostRuleResult
        {
            Name = "Repeated posting"
        };


        if (string.IsNullOrWhiteSpace(context.Application.ExternalJobId))
        {
            result.Description =
                "Brak identyfikatora ogłoszenia.";

            return result;
        }


        var sameJobApplications = context.AllApplications
            .Where(x =>
                x.ExternalJobId == context.Application.ExternalJobId)
            .ToList();


        var count = sameJobApplications.Count;


        if (count <= 1)
        {
            result.Description =
                "Brak ponownych aplikacji na to samo ogłoszenie.";

            return result;
        }


        result.Triggered = true;
        result.Score = 10;

        result.Description =
            $"To samo ogłoszenie wystąpiło {count} razy.";

        return result;
    }
}