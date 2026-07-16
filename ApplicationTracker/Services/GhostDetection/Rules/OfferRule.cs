using ApplicationTracker.Models;
using ApplicationTracker.Models.Analysis;

namespace ApplicationTracker.Services.GhostDetection.Rules;

public class OfferRule : IGhostRule
{
    public GhostRuleResult Evaluate(
        GhostAnalysisContext context)
    {
        var result = new GhostRuleResult
        {
            Name = "Offer received"
        };


        var hadOffer = context.Application.History
            .Any(x => x.Status == ApplicationStatus.Offer);


        if (hadOffer)
        {
            result.Triggered = true;
            result.Score = -50;

            result.Description =
                "Firma przedstawiła ofertę.";

            return result;
        }


        result.Description =
            "Brak oferty.";

        return result;
    }
}