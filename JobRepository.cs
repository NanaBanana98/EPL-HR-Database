using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    public class JobRepository
    {
        private static readonly Dictionary<string, Job> Jobs = new Dictionary<string, Job>();

        public static Job Get(string title)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            bool success = Jobs.TryGetValue(title, out Job job);

            if (success)
            {
                return job;
            }

            return null;
        }

        public static int Save(string title, Job job)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (job is null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            if (Jobs.ContainsKey(title))
            {
                Jobs.Remove(title);
            }

            Jobs.Add(title, job);

            return -1;
        }

    }
}
