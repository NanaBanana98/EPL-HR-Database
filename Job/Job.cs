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
        public Job(string title, string description)
        {
            this.Title = title;
            this.Description = description;
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
        /// List of all transactions on this event 
        /// </summary>
        public List<IJobEvents> Events { get; private set; }

        #region Actions

        /// <summary>
        /// Updates the event Title/Description to match the given value
        /// </summary>
        /// <param name="Title">New title value</param>
        /// <param name="Description">New Description value</param>
        /// <returns>
        /// -1 -> null title value
        /// -2 -> null description value
        /// 0 -> success
        /// </returns>
        public int UpdateJob(string Title, string Description)
        {
            //Check Domain Requirements
            if (Title is null)
            {
                return -1;
            }

            if (Description is null)
            {
                return -2;
            }

            //Comments
            string Comment = "";
            Comment += Title.Equals(this.Title) ? "" : "Title Updated. ";
            Comment += Description.Equals(this.Description) ? "" : "Description Updated. ";

            //Update state
            this.Title = Title;
            this.Description = Description;

            //Add 'UpdateJob' Event
            Events.Add(new UpdateJob((int)Id, this.Title, this.Description, DateTime.Now, Comment));
            return 0;
        }

        #endregion

        public override string ToString()
        {
            return "Title: "+Title+"\n"+"Description: "+Description+"\n";
        }
    }
}
