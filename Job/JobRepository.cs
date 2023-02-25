using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    public class JobRepository
    {
        private static readonly Dictionary<int, Job> Jobs = new Dictionary<int, Job>();

        public static Job Get(int id)
        {

            bool success = Jobs.TryGetValue(id, out Job job);

            if (success)
            {
                return job;
            }

            return null;
        }

        public static List<Job> GetAll()
        {
            return Jobs.Values.ToList();
        }

        public static int Save(int? id, Job job)
        {
            if(id != null)
            {
                if(Jobs.ContainsKey((int)id))
                {
                    Jobs[(int)id] = job;
                    return 0;
                }
            }
            else
            {
                Jobs.Add((int)id, job);
                return 0;
            }

            return -1;

        }

    }
}
