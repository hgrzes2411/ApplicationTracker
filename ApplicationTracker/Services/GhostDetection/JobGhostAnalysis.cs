using ApplicationTracker.Models;
using ApplicationTracker.Services;

namespace ApplicationTracker.Models.GhostDetection;

public class JobGhostAnalysis
{
    public Guid JobId { get; set; }

    public int Score { get; set; }

    public string RiskLevel { get; set; } = "";

    public GhostAnalysisResult Result { get; set; } = new();
}