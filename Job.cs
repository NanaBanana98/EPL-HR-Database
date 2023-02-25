using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    /// <summary>
    /// Represents a Job Table Record
    /// </summary>
    public class Job
    {
        public Job(int id, string title, string description, DateTime lastUpdated, List<IJobEvents> jobEvents)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.LastUpdated = lastUpdated;
            this.TableTransactions = jobEvents;
        }

        #region Model State Representation
        public int? Id { get; private set; } = null;


        /// <summary>
        /// Job Title
        /// </summary>
        public String Title { get; private set; } = "";

        /// <summary>
        /// Job Description
        /// </summary>
        public String Description { get; private set; } = "";

        /// <summary>
        /// Indicates the last time an event was updated
        /// </summary>
        public DateTime LastUpdated { get; private set; } = DateTime.Now;
        #endregion

        /// <summary>
        /// Transactions that occcured in order for this job to get to its current state
        /// </summary>
        public List<IJobEvents> TableTransactions { get; private set; } = new List<IJobEvents>();

        /// <summary>
        /// Updates the title field of the Job
        /// </summary>
        /// <param name="title">Updated title value</param>
        /// <returns>true if updated; false otherwise</returns>
        public bool SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            this.Title = title;
            this.LastUpdated = DateTime.UtcNow;
            return true;
        }

        

        /// <summary>
        /// Updates the description field of the Job
        /// </summary>
        /// <param name="description">Updated description value</param>
        /// <returns>true if updated; false otherwise</returns>
        public bool SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            this.Description = description;
            this.LastUpdated = DateTime.UtcNow;
            return true;
        }

        public override string ToString()
        {
            return "{\n\tID: "+this.Id+"\n\tTitle: "+Title+"\n\tDescription: "+Description+"\n}\n";
        }
    }
}
