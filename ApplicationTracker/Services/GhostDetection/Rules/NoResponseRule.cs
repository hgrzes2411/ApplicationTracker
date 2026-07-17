using ApplicationTracker.Models;

namespace ApplicationTracker.Services.GhostDetection.Rules;

public class NoResponseRule : IGhostRule
{
    public GhostRuleResult Evaluate(
        GhostAnalysisContext context)
    {
        var result = new GhostRuleResult
        {
            Name = "No response"
        };


        var history = context.Application.History
            .OrderByDescending(x => x.Date)
            .FirstOrDefault();


        if (history == null)
        {
            result.Description =
                "Brak historii statusów.";

            return result;
        }


        if (history.Status != ApplicationStatus.Applied)
        {
            result.Description =
                "Firma wykonała akcję w procesie.";

            return result;
        }


        var days =
            context.Today.DayNumber -
            history.Date.DayNumber;


        if (days >= 60)
        {
            result.Triggered = true;
            result.Score = 30;

            result.Description =
                $"Brak odpowiedzi od {days} dni.";

            return result;
        }


        if (days >= 30)
        {
            result.Triggered = true;
            result.Score = 20;

            result.Description =
                $"Brak odpowiedzi od {days} dni.";

            return result;
        }


        result.Description =
            $"Aplikacja oczekuje {days} dni.";

        return result;
    }
}