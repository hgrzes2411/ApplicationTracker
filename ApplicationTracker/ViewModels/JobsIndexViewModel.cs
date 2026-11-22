using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using ApplicationTracker.Services.GhostDetection;
using System.Collections.Generic;

namespace ApplicationTracker.ViewModels;

public class JobsIndexViewModel
{
    public IReadOnlyCollection<JobApplication> Jobs { get; init; } = [];

    public JobFilter Filter { get; init; } = new();

    public IReadOnlyCollection<string> Companies { get; init; } = [];

    public IReadOnlyDictionary<Guid, GhostAnalysisResult> GhostAnalyses { get; init; } =
        new Dictionary<Guid, GhostAnalysisResult>();
}
