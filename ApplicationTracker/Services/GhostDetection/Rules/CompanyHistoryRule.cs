namespace ApplicationTracker.Services.GhostDetection.Rules;

public class CompanyHistoryRule : IGhostRule
{
    public GhostRuleResult Evaluate(
        GhostAnalysisContext context)
    {
        var companyApplications = context.AllApplications
            .Where(x =>
                x.NormalizedCompany ==
                context.Application.NormalizedCompany)
            .ToList();


        var recruitmentCycles = companyApplications
            .Select(x =>
                string.IsNullOrWhiteSpace(x.ExternalJobId)
                    ? x.Id.ToString()
                    : x.ExternalJobId)
            .Distinct()
            .Count();


        var result = new GhostRuleResult
        {
            Name = "Company history"
        };


        result.Score = recruitmentCycles switch
        {
            >= 5 => 40,
            4 => 30,
            3 => 20,
            _ => 0
        };


        result.Triggered = result.Score > 0;


        result.Description =
            $"Firma posiada {recruitmentCycles} cykle rekrutacyjne.";


        return result;
    }
}