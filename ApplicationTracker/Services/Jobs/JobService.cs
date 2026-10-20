using ApplicationTracker.Models;
using ApplicationTracker.Models.Filters;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationTracker.Services.Jobs
{
    public class JobService
    {
        private readonly JsonRepository _repository;

        public JobService(JsonRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JobApplication>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(JobApplication job)
        {
            var jobs = await _repository.GetAllAsync();

            job.Id = Guid.NewGuid();
            job.ExternalJobId = GenerateExternalJobId(job);

            jobs.Add(job);

            await _repository.SaveAsync(jobs);
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

        public async Task DeleteAsync(Guid id)
        {
            var jobs = await _repository.GetAllAsync();

            var job = jobs.FirstOrDefault(x => x.Id == id);

            if (job != null)
            {
                jobs.Remove(job);

                await _repository.SaveAsync(jobs);
            }
        }

        public async Task UpdateAsync(JobApplication updatedJob)
        {
            var jobs = await _repository.GetAllAsync();

            var existingJob = jobs.FirstOrDefault(x => x.Id == updatedJob.Id);

            if (existingJob == null)
            {
                return;
            }

            existingJob.UpdateFrom(updatedJob);

            await _repository.SaveAsync(jobs);
        }
    }
}