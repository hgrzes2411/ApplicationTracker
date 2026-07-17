using ApplicationTracker.Models;

namespace ApplicationTracker.Services;

public class GhostJobAnalyzer
{
    public GhostAnalysisResult Analyze(
        JobApplication application,
        IEnumerable<JobApplication> allApplications)
    {
        return new GhostAnalysisResult();
    }
}