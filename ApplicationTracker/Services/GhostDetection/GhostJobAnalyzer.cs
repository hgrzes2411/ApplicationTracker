using ApplicationTracker.Models;

namespace ApplicationTracker.Services.GhostDetection;

public class GhostJobAnalyzer
{
    public GhostAnalysisResult Analyze(
        JobApplication application,
        IEnumerable<JobApplication> allApplications)
    {
        return new GhostAnalysisResult();
    }
}
