using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    public class JobRepo
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

        public void Save(List<Job> jobs)
        {
            sql = "DELETE FROM Jobs";
            if(OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);
                cmd.ExecuteNonQuery();

                sql = @"INSERT INTO Jobs (ID, Title, Description,[Last Updated]) VALUES (?,?,?,?);";


                foreach (var job in jobs)
                {
                    OleDbCommand cmd2 = new OleDbCommand(sql, dbCon);
                    cmd2.Parameters.Add("ID", OleDbType.Numeric).Value = (int)job.Id;
                    cmd2.Parameters.Add("Title", OleDbType.VarChar).Value = job.Title;
                    cmd2.Parameters.Add("Description", OleDbType.VarChar).Value = job.Description;
                    cmd2.Parameters.Add("[Last Updated]", OleDbType.Date).Value = job.LastUpdated;

                    cmd2.ExecuteNonQuery();
                }


                dbCon.Close();
            }
        }

        public Job Get(int Id)
        {
            Jobs jobsEvents = new Jobs();
            Job job = null;
            sql = "SELECT * FROM Job WHERE ID=" + Id;

            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);

                dr1 = cmd.ExecuteReader();

                while (dr1.Read())
                {
                    job = new Job(Int32.Parse(dr1[0].ToString()),dr1[1].ToString(),dr1[2].ToString(),Convert.ToDateTime(dr1[3].ToString()), jobsEvents.TransactionHistory(Int32.Parse(dr1[0].ToString())));
                }

                dbCon.Close();
            }
            return job;
        }

        public List<Job> GetAll()
        {
            Jobs jobsEvents = new Jobs();
            List<Job> jobs = new List<Job>();
            sql = "SELECT * FROM Job";

            if (OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(sql, dbCon);

                dr1 = cmd.ExecuteReader();

                while (dr1.Read())
                {
                    jobs.Add(new Job(Int32.Parse(dr1[0].ToString()), dr1[1].ToString(), dr1[2].ToString(), Convert.ToDateTime(dr1[3].ToString()), jobsEvents.TransactionHistory(Int32.Parse(dr1[0].ToString()))));
                }
                dbCon.Close();
            }
            return jobs;
        }
    }
}
