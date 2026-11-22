using ApplicationTracker.Data;
using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationTracker.Services.Dashboard
{
    public class JobService
    {
        private readonly ApplicationDbContext _dbContext;

        public JobService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.JobApplications
                .Include(job => job.History)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AddAsync(JobApplication job, CancellationToken cancellationToken = default)
        {
            if (job.Id == Guid.Empty)
            {
                job.Id = Guid.NewGuid();
            }

            job.ExternalJobId = GenerateExternalJobId(job);

            var duplicateExists = await _dbContext.JobApplications
                .AnyAsync(application => application.ExternalJobId == job.ExternalJobId, cancellationToken);

            if (duplicateExists)
            {
                return false;
            }

            foreach (var history in job.History)
            {
                history.Id = history.Id == Guid.Empty ? Guid.NewGuid() : history.Id;
                history.JobApplicationId = job.Id;
            }

            _dbContext.JobApplications.Add(job);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddStatusAsync(
            Guid jobId,
            StatusHistory status,
            CancellationToken cancellationToken = default)
        {
            var job = await _dbContext.JobApplications
                .FirstOrDefaultAsync(application => application.Id == jobId, cancellationToken);

            if (job is null)
            {
                return false;
            }

            status.Id = status.Id == Guid.Empty ? Guid.NewGuid() : status.Id;
            status.JobApplicationId = jobId;

            _dbContext.StatusHistories.Add(status);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static string GenerateExternalJobId(JobApplication job)
        {
            if (string.IsNullOrWhiteSpace(job.Url))
            {
                return job.Id.ToString("N");
            }

            var normalizedUrl = job.Url.Trim().ToLowerInvariant();
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalizedUrl));

            return Convert.ToHexString(hash)[..12];
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _dbContext.JobApplications
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (job != null)
            {
                _dbContext.JobApplications.Remove(job);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(JobApplication updatedJob, CancellationToken cancellationToken = default)
        {
            var existingJob = await _dbContext.JobApplications
                .Include(job => job.History)
                .FirstOrDefaultAsync(x => x.Id == updatedJob.Id, cancellationToken);

            if (existingJob == null)
            {
                return;
            }

            existingJob.Company = updatedJob.Company;
            existingJob.Position = updatedJob.Position;
            existingJob.Location = updatedJob.Location;
            existingJob.WorkMode = updatedJob.WorkMode;
            existingJob.SalaryFrom = updatedJob.SalaryFrom;
            existingJob.SalaryTo = updatedJob.SalaryTo;
            existingJob.ApplicationDate = updatedJob.ApplicationDate;
            existingJob.Source = updatedJob.Source;
            existingJob.Url = updatedJob.Url;
            existingJob.Notes = updatedJob.Notes;

            _dbContext.StatusHistories.RemoveRange(existingJob.History);
            existingJob.History = updatedJob.History.Select(history => new StatusHistory
            {
                Id = history.Id == Guid.Empty ? Guid.NewGuid() : history.Id,
                JobApplicationId = existingJob.Id,
                Status = history.Status,
                Date = history.Date,
                Note = history.Note
            }).ToList();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}