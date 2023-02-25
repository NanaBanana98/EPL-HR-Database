using System;
using System.Collections.Generic;
using System.Linq;

namespace EPL_HR_Database
{

    /// <summary>
    /// Used to manage Jobs
    /// </summary>
    public class Jobs
    {
        /// <summary>
        /// Persists  Jobs Data and Gets State
        /// </summary>
        readonly EventStore eventStore = new EventStore();

        readonly JobRepo jobRepo = new JobRepo();

        readonly ModelBuilder modelBuilder = new ModelBuilder();

        /// <summary>
        /// Creates a new job in memory
        /// </summary>
        /// <param name="title">Job Title</param>
        /// <param name="description">Job description</param>
        public void CreateJob(string title, string description)
        {
            eventStore.AddEvent(new CreateJob(0,title,description, DateTime.UtcNow, ""));
            jobRepo.Save(GetJobs());
        }

        /// <summary>
        /// Updates the value of a job in memory
        /// </summary>
        /// <param name="job">Job with updated values</param>
        public void UpdateJob(Job job)
        {
            eventStore.AddEvent(new UpdateJob((int)job.Id, job.Title, job.Description, job.LastUpdated, ""));
            jobRepo.Save(GetJobs());
        }

        /// <summary>
        /// Removes a job
        /// </summary>
        /// <param name="job">Job to remove</param>
        public void DecomissionJob(Job job)
        {
            eventStore.AddEvent(new DecommisionJob((int)job.Id,job.Title,job.Description,DateTime.UtcNow,""));
            jobRepo.Save(GetJobs());
        }

        /// <summary>
        /// Gets a job saved in memory
        /// </summary>
        /// <param name="id">Id of the job to fetch</param>
        public Job GetJob(int id)
        {
            return modelBuilder.GetModel(TransactionHistory(id), 0);
        }

        /// <summary>
        /// Gets a job saved in memory
        /// </summary>
        /// <param name="id">Id of the job to fetch</param>
        /// <param name="pop">Pervious version to display</param>
        /// <returns>Job fetched with given ID</returns>
        public Job GetJob(int id, int pop)
        {
            return modelBuilder.GetModel(TransactionHistory(id), pop);
        }

        /// <summary>
        /// Gets the history of all memory transactions for the event with matching ID
        /// </summary>
        /// <param name="id">ID of event whose histpry to query</param>
        /// <returns>List of Events applied to a job</returns>
        public List<IJobEvents> TransactionHistory(int id)
        {
            return eventStore.GetTransactionsForJob(id);
        }

        /// <summary>
        /// Gets a list of all jobs saved in memory
        /// </summary>
        public List<Job> GetJobs()
        {
            return modelBuilder.GetModels(eventStore.GetTransactions().Values.ToList());
        }
    }
}
