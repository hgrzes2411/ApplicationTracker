using ApplicationTracker.Models.Analysis;

namespace ApplicationTracker.Services;

public class GhostAnalysisResult
{
    public bool IsGhost => Score >= 60;

    public int Score { get; set; }

    public GhostRiskLevel RiskLevel { get; set; }

    public int ApplicationsToCompany { get; set; }

    public int RecruitmentCycles { get; set; }

    public int ResponsesReceived { get; set; }

    public int DaysWithoutResponse { get; set; }

    public string Summary { get; set; } = "";

    public List<GhostRuleResult> Rules { get; set; } = [];
}