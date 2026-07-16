using ApplicationTracker.Models;
using ApplicationTracker.Models.Analysis;

namespace ApplicationTracker.Services.GhostDetection.Rules;

public class InterviewRule : IGhostRule
{
    public GhostRuleResult Evaluate(
        GhostAnalysisContext context)
    {
        var result = new GhostRuleResult
        {
            Name = "Interview happened"
        };

        var hadInterview = context.Application.History
            .Any(x => x.Status == ApplicationStatus.Interview);


        if (hadInterview)
        {
            result.Triggered = true;
            result.Score = -30;

            result.Description =
                "Proces zawierał etap rozmowy kwalifikacyjnej.";

            return result;
        }


        result.Description =
            "Brak informacji o rozmowie.";

        return result;
    }
}