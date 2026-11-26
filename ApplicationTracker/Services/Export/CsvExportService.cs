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
    $"{Escape(job.Company)},{Escape(job.Position)},{Escape(job.CurrentStatus.ToString())},{Escape(job.Source)},{Escape(job.WorkMode.ToString())},{Escape(job.ApplicationDate.ToString("yyyy-MM-dd"))}"
);
        }


        return string.Join(
            Environment.NewLine,
            csv
        );
    }

    private static string Escape(string? value)
    {
        return $"\"{value?.Replace("\"", "\"\"")}\"";
    }
}