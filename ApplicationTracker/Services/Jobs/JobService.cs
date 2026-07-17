using ApplicationTracker.Models;

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

            jobs.Add(job);

            await _repository.SaveAsync(jobs);
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
