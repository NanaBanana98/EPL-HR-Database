using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    /// <summary>
    /// Connects the Job Transaction Data to Memory
    /// </summary>
    public class EventStore
    {
        private static string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/Users/Ariana/Documents/EPL-HR-DB.accdb";
        private static string sql;
        private static OleDbConnection dbCon = new OleDbConnection();
        private static OleDbDataReader dr1;

        public static bool OpenConnection()
        {
            try
            {
                dbCon.ConnectionString = connString;
                dbCon.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        /// <summary>
        /// Gets a list of transactions for a job
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <returns>List of job transactions</returns>
        public List<IJobEvents> GetTransactionsForJob(int Id)
        {
            List<IJobEvents> jobEvents = new List<IJobEvents>();

            sql = "SELECT * FROM [Job Table Transactions] WHERE JobID=" + Id;
            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);

                dr1 = cmd.ExecuteReader();

                while (dr1.Read())
                {
                    switch (dr1[1].ToString())
                    {
                        case "CREATED":
                            jobEvents.Add(new CreateJob(Id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        case "UPDATED":
                            jobEvents.Add(new UpdateJob(Id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        case "REMOVED":
                            jobEvents.Add(new DecommisionJob(Id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        default:
                            break;
                    }
                }
            }
            dbCon.Close();

            return jobEvents;
        }

        /// <summary>
        /// Gets a list of a list of transactions. Each inner list is grouped by ID
        /// </summary>
        /// <returns>List of all transactions saved, grouped by job id</returns>
        public IDictionary<int, List<IJobEvents>> GetTransactions()
        {
            IDictionary<int, List<IJobEvents>> jobEvents = new Dictionary<int, List<IJobEvents>>();
            sql = "SELECT * FROM [Job Table Transactions]";

            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);

                dr1 = cmd.ExecuteReader();

                while (dr1.Read())
                {
                    int id = Int32.Parse(dr1[6].ToString());
                    if(jobEvents.ContainsKey(id) == false)
                    {
                        jobEvents.Add(id, new List<IJobEvents>());
                    }

                    switch (dr1[1].ToString())
                    {
                        case "CREATED":
                            jobEvents[id].Add(new CreateJob(id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        case "UPDATED":
                            jobEvents[id].Add(new UpdateJob(id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        case "REMOVED":
                            jobEvents[id].Add(new DecommisionJob(id, dr1[3].ToString(), dr1[4].ToString(), Convert.ToDateTime(dr1[5].ToString()), dr1[2].ToString()));
                            break;
                        default:
                            break;
                    }
                }
            }

            dbCon.Close();
            return jobEvents;
        }

        /// <summary>
        /// Adds an event transaaction to memory
        /// </summary>
        /// <param name="evnt">New event</param>
        public void AddEvent(IJobEvents evnt)
        {
            switch (evnt)
            {
                case CreateJob createJob:
                    AddCreateJob(createJob);
                    break;

                case DecommisionJob decommisionJob:
                    AddDecommisionJob(decommisionJob);
                    break;

                case UpdateJob updateJob:
                    AddUpdateJob(updateJob);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Adds a new 'UpdateJob' Transaction to memory
        /// </summary>
        /// <param name="updateJob">Transaction to add</param>
        private void AddUpdateJob(UpdateJob updateJob)
        {
            sql = @"INSERT INTO [Job Table Transactions] (TransactionDate,[Action],Comments,Title,Description,JobID) VALUES (?,'UPDATED',?,?,?,?);";
            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);
                cmd.Parameters.Add("TransactionDate", OleDbType.Date).Value = updateJob.Updated;
                cmd.Parameters.Add("Comments", OleDbType.VarChar).Value = updateJob.Comments;
                cmd.Parameters.Add("Title", OleDbType.VarChar).Value = updateJob.Title;
                cmd.Parameters.Add("Description", OleDbType.VarChar).Value = updateJob.Description;
                cmd.Parameters.Add("JobID", OleDbType.Integer).Value = updateJob.ID;

                cmd.ExecuteNonQuery();
                dbCon.Close();
            }
        }

        /// <summary>
        /// Adds a DecomissionJob transaction to memory
        /// </summary>
        /// <param name="decommisionJob">Transaction to add</param>
        private void AddDecommisionJob(DecommisionJob decommisionJob)
        {
            sql = @"INSERT INTO [Job Table Transactions] (TransactionDate,[Action],Comments,Title,Description,JobID) VALUES (?,'REMOVED',?,?,?,?);";
            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);
                cmd.Parameters.Add("TransactionDate", OleDbType.Date).Value = decommisionJob.Removed;
                cmd.Parameters.Add("Comments", OleDbType.VarChar).Value = decommisionJob.Comments;
                cmd.Parameters.Add("Title", OleDbType.VarChar).Value = decommisionJob.Title;
                cmd.Parameters.Add("Description", OleDbType.VarChar).Value = decommisionJob.Description;
                cmd.Parameters.Add("JobID", OleDbType.Integer).Value = decommisionJob.Id;

                cmd.ExecuteNonQuery();
                dbCon.Close();
            }
        }

        /// <summary>
        /// Adds a create a job transactions to memeory
        /// </summary>
        /// <param name="createJob">Transaction to add</param>
        private void AddCreateJob(CreateJob createJob)
        {
            Random rnd = new Random();
            int id = rnd.Next(0, Int32.MaxValue);
            sql = "SELECT COUNT(*) FROM [Job Table Transactions] WHERE JobID=" + id;
            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);
                int rowCount = (int)cmd.ExecuteScalar();

                while(rowCount > 0)
                {
                    id = rnd.Next(0, Int32.MaxValue);
                }

                sql = @"INSERT INTO [Job Table Transactions] (TransactionDate,[Action],Comments,Title,Description,JobID) VALUES (?,'CREATED',?,?,?,?);";

                OleDbCommand cmd2 = new OleDbCommand(sql, dbCon);
                cmd2.Parameters.Add("TransactionDate", OleDbType.Date).Value = createJob.Created;
                cmd2.Parameters.Add("Comments", OleDbType.VarChar).Value = createJob.Comments;
                cmd2.Parameters.Add("Title", OleDbType.VarChar).Value = createJob.Title;
                cmd2.Parameters.Add("Description", OleDbType.VarChar).Value = createJob.Description;
                cmd2.Parameters.Add("JobID", OleDbType.Integer).Value = id;

                cmd2.ExecuteNonQuery();

                dbCon.Close();
            }
        }
    }
}
