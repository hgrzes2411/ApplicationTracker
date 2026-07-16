using ApplicationTracker.Models;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace ApplicationTracker.Services;

public class JsonRepository
{
    private readonly string _filePath = "Data/jobs.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public async Task<List<JobApplication>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<JobApplication>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<JobApplication>();
        }

        return JsonSerializer.Deserialize<List<JobApplication>>(json, _options)
               ?? new List<JobApplication>();
    }


    public async Task SaveAsync(List<JobApplication> jobs)
    {
        var json = JsonSerializer.Serialize(jobs, _options);

        await File.WriteAllTextAsync(_filePath, json);
    }
}