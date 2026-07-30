using ApplicationTracker.Models;

namespace ApplicationTracker.Services.Export;

public class CsvExportService
{
    public string GenerateCsv(IEnumerable<JobApplication> jobs)
    {
        var csv = new List<string>();

        csv.Add(
     "Company,Position,Status,Source,WorkMode,ApplicationDate"
 );


        foreach (var job in jobs)
        {
            csv.Add(
    $"\"{job.Company}\"," +
    $"\"{job.Position}\"," +
    $"\"{job.Status}\"," +
    $"\"{job.Source}\"," +
    $"\"{job.WorkMode}\"," +
    $"\"{job.ApplicationDate:yyyy-MM-dd}\""
);
        }


        return string.Join(
            Environment.NewLine,
            csv
        );
    }
}