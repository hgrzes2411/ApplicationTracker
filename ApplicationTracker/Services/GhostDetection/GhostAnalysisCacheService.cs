using ApplicationTracker.Models;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.GhostDetection;

public class GhostAnalysisCacheService
{
    private readonly GhostDetector _ghostDetector;

    private Dictionary<Guid, GhostAnalysisResult> _cache = new();

    public GhostAnalysisCacheService(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }


    public Dictionary<Guid, GhostAnalysisResult> AnalyzeAll(
        IEnumerable<JobApplication> jobs)
    {
        _cache = jobs.ToDictionary(
            job => job.Id,
            job => _ghostDetector.Analyze(job, jobs));

        return _cache;
    }


    public GhostAnalysisResult Get(Guid id)
    {
        return _cache.TryGetValue(id, out var result)
            ? result
            : new GhostAnalysisResult();
    }
}