using ApplicationTracker.Models;
using ApplicationTracker.Models.Dashboard;
using ApplicationTracker.Services.GhostDetection;

namespace ApplicationTracker.Services.Dashboard;

public class RecommendationCalculator
{
    private readonly GhostDetector _ghostDetector;


    public RecommendationCalculator(
        GhostDetector ghostDetector)
    {
        _ghostDetector = ghostDetector;
    }


    public RecommendationDashboardModel Calculate(
        IReadOnlyCollection<JobApplication> jobs)
    {
        var items = jobs
            .Select(job =>
            {
                var analysis = _ghostDetector.Analyze(job, jobs);

                var recommendation = new RecommendationItemModel
                {
                    Company = job.Company,
                    GhostScore = analysis.Score
                };


                if (analysis.Score >= 70)
                {
                    recommendation.Level =
                        RecommendationLevel.Critical;

                    recommendation.Message =
                        "Wysokie ryzyko ghostingu. Rozważ zakończenie procesu.";
                }
                else if (analysis.Score >= 40)
                {
                    recommendation.Level =
                        RecommendationLevel.Warning;

                    recommendation.Message =
                        "Proces wymaga obserwacji. Brak wyraźnych oznak zakończenia.";
                }
                else
                {
                    recommendation.Level =
                        RecommendationLevel.Info;

                    recommendation.Message =
                        "Proces wygląda prawidłowo.";
                }


                return recommendation;
            })
            .OrderByDescending(x => x.GhostScore)
            .ToList();


        return new RecommendationDashboardModel
        {
            Items = items
        };
    }
}