using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using ApplicationTracker.Services.Dashboard;
using ApplicationTracker.Services.Export;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Controllers;

public class JobsController : Controller
{
    private readonly JobService _jobService;
    private readonly JobFilteringService _jobFilteringService;
    private readonly CsvExportService _csvExportService;
    private readonly GhostAnalysisCacheService _ghostAnalysisCacheService;

    public JobsController(
        JobService jobService,
        JobFilteringService jobFilteringService,
        CsvExportService csvExportService,
        GhostAnalysisCacheService ghostAnalysisCacheService)
    {
        _jobService = jobService;
        _jobFilteringService = jobFilteringService;
        _csvExportService = csvExportService;
        _ghostAnalysisCacheService = ghostAnalysisCacheService;
    }

    public async Task<IActionResult> Index(JobFilter filter, CancellationToken cancellationToken)
    {
        var allJobs = await _jobService.GetAllAsync(cancellationToken);
        var filteredJobs = _jobFilteringService.FilterJobs(allJobs, filter).ToList();
        var analyses = _ghostAnalysisCacheService.AnalyzeAll(allJobs);

        return View(new JobsIndexViewModel
        {
            Jobs = filteredJobs,
            Filter = filter,
            Companies = allJobs.Select(job => job.Company).Distinct().OrderBy(company => company).ToList(),
            GhostAnalyses = analyses
        });
    }

    [HttpGet]
    public async Task<IActionResult> Export(CancellationToken cancellationToken)
    {
        var jobs = await _jobService.GetAllAsync(cancellationToken);
        var csv = _csvExportService.GenerateCsv(jobs);
        return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"applications_export_{DateTime.Now:yyyy-MM-dd}.csv");
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var job = (await _jobService.GetAllAsync(cancellationToken))
            .FirstOrDefault(application => application.Id == id);

        return job is null ? NotFound() : View(job);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new JobApplicationFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobApplicationFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var job = ToEntity(model);
        job.History.Add(new StatusHistory
        {
            Status = ApplicationStatus.Applied,
            Date = job.ApplicationDate
        });

        var added = await _jobService.AddAsync(job, cancellationToken);

        if (!added)
        {
            ModelState.AddModelError(nameof(model.Url), "Aplikacja z tym adresem URL już istnieje.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var job = (await _jobService.GetAllAsync(cancellationToken))
            .FirstOrDefault(application => application.Id == id);

        return job is null ? NotFound() : View(ToViewModel(job));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, JobApplicationFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingJob = (await _jobService.GetAllAsync(cancellationToken))
            .FirstOrDefault(application => application.Id == id);

        if (existingJob is null)
        {
            return NotFound();
        }

        model.History = existingJob.History.Select(history => new StatusHistoryViewModel
        {
            Id = history.Id,
            Status = history.Status,
            Date = history.Date,
            Note = history.Note
        }).ToList();

        await _jobService.UpdateAsync(ToEntity(model), cancellationToken);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStatus(Guid id, StatusHistoryViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Edit), new { id });
        }

        var added = await _jobService.AddStatusAsync(id, new StatusHistory
        {
            Status = model.Status,
            Date = model.Date,
            Note = model.Note
        }, cancellationToken);

        return added
            ? RedirectToAction(nameof(Edit), new { id })
            : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var job = (await _jobService.GetAllAsync(cancellationToken))
            .FirstOrDefault(application => application.Id == id);

        return job is null ? NotFound() : View(job);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        await _jobService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private static JobApplication ToEntity(JobApplicationFormViewModel model)
    {
        return new JobApplication
        {
            Id = model.Id,
            Company = model.Company,
            Position = model.Position,
            Location = model.Location,
            WorkMode = model.WorkMode!.Value,
            SalaryFrom = model.SalaryFrom,
            SalaryTo = model.SalaryTo,
            ApplicationDate = model.ApplicationDate,
            Source = model.Source,
            Url = model.Url,
            Notes = model.Notes,
            History = model.History.Select(history => new StatusHistory
            {
                Id = history.Id,
                Status = history.Status,
                Date = history.Date,
                Note = history.Note
            }).ToList()
        };
    }

    private static JobApplicationFormViewModel ToViewModel(JobApplication job)
    {
        return new JobApplicationFormViewModel
        {
            Id = job.Id,
            Company = job.Company,
            Position = job.Position,
            Location = job.Location,
            WorkMode = job.WorkMode,
            SalaryFrom = job.SalaryFrom,
            SalaryTo = job.SalaryTo,
            ApplicationDate = job.ApplicationDate,
            Source = job.Source,
            Url = job.Url,
            Notes = job.Notes,
            History = job.History.Select(history => new StatusHistoryViewModel
            {
                Id = history.Id,
                Status = history.Status,
                Date = history.Date,
                Note = history.Note
            }).ToList()
        };
    }
}
