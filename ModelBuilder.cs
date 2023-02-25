using System;
using System.Collections.Generic;


namespace EPL_HR_Database
{
    /// <summary>
    /// Builds Job Models based off of Job Transactions provided
    /// </summary>
    public class ModelBuilder
    {

        /// <summary>
        /// List of job models created using lists of job transactions
        /// </summary>
        /// <param name="jobEvents">List of Transactions for events</param>
        /// <returns>A list of models corresponding to given job transactions</returns>
        public List<Job> GetModels(IList<List<IJobEvents>> jobEvents)
        {
            List<Job> jobs = new List<Job>();
            foreach (List<IJobEvents> item in jobEvents)
            {
                jobs.Add(GetModel(item, 0));
            }
            return jobs;
        }

        /// <summary>
        /// Gets a Model based on given transactions
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="pop">Indicates which of the later versions of the Model to hide (show what the model looked like in the past)</param>
        /// <returns>The Job model built using the given params</returns>
        public Job GetModel(List<IJobEvents> transactions, int pop)
        {
            Job job = null;
            List<IJobEvents> evs = transactions;

            DateTime mostRecent = DateTime.MinValue;

            //show at least the first transaction
            if(pop > transactions.Count)
            {
                pop = transactions.Count - 1;
            }

            //remove the newest events
            evs = evs.GetRange(pop, evs.Count);

            //Cycle through and build model based on transactions
            foreach (var item in evs)
            {
                switch (item)
                {
                    case CreateJob createJob:
                        if (mostRecent < createJob.Created)
                        {
                            job = new Job(createJob.Id, createJob.Title, createJob.Description, createJob.Created, transactions);
                        }
                        break;
                    case UpdateJob updateJob:
                        if (mostRecent < updateJob.Updated)
                        {
                            job = new Job(updateJob.ID, updateJob.Title, updateJob.Description, updateJob.Updated, transactions);
                        }
                        break;
                    case DecommisionJob decommisionJob:
                        if (mostRecent < decommisionJob.Removed)
                        {
                            // job = new Job(decommisionJob.Id, decommisionJob.Title, decommisionJob.Description, decommisionJob.Removed);
                            job = null;
                        }
                        break;
                    default:
                        break;
                }
            }

            return job;
        }
    }
}
