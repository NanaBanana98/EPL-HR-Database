/**
using System;

namespace EPL_HR_Database
{
    class Program
    {
        static void Main()
        {

            Jobs jobs = new Jobs();
            bool continueLoop = true;
            int option;

            while (continueLoop)
            {
                Menu();
                option = Int32.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Enter title:\n");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter Description:\n");
                        string description = Console.ReadLine();
                        jobs.CreateJob(title,description);         
                        break;

                    case 1:
                        Console.WriteLine("Enter Id:\n");
                        int id = Int32.Parse(Console.ReadLine());

                        Job job = jobs.GetJob(id);

                        Console.WriteLine("Enter Updated Title:\n");
                        title = Console.ReadLine();
                        Console.WriteLine("Enter Description:\n");
                        description = Console.ReadLine();

                        job.SetTitle(title);
                        job.SetDescription(description);
                        jobs.UpdateJob(job);
                        break;

                    case 2:
                        Console.WriteLine("Enter Id:\n");
                        id = Int32.Parse(Console.ReadLine());

                        job = jobs.GetJob(id);
                        jobs.DecomissionJob(job);
                        break;

                    case 3:
                        Console.WriteLine("Enter Id:\n");
                        id = Int32.Parse(Console.ReadLine());
                        Console.WriteLine(jobs.GetJob(id));
                        break;

                    case 4:
                        foreach(var item in jobs.GetJobs()){
                            Console.WriteLine(item);
                        }
                        break;

                    case 5:
                        Console.WriteLine("Enter Id:\n");
                        id = Int32.Parse(Console.ReadLine());
                        foreach (IJobEvents item in jobs.TransactionHistory(id))
                        {
                            switch (item)
                            {
                                case CreateJob createJob:
                                    Console.WriteLine("CREATED Date: " + createJob.Created + " Title: " + createJob.Title + " Description: " + createJob.Description + " Comments: " + createJob.Comments + "\n");
                                    break;
                                case UpdateJob updateJob:
                                    Console.WriteLine("UPDATE Date: "+updateJob.Updated+" Title: "+ updateJob.Title+ " Description: "+ updateJob.Description+" Comments: "+updateJob.Comments+"\n");
                                    break;
                                case DecommisionJob removeJob:
                                    Console.WriteLine("REMOVED Date: " + removeJob.Removed + " Title: " + removeJob.Title + " Comments: " + removeJob.Comments+"\n");
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    
                    case -1:
                        return;

                    default:
                        break;
                }
            }
        }

        static void Menu()
        {
            Console.WriteLine(
                "Select an Option:\n"+
                "0: Create a job\n"+
                "1: Update a job\n"+
                "2: Remove a job\n"+
                "3: List a job\n"+
                "4: List all jobs\n"+
                "5: List job transactions\n"
          );
        }
    }
}**/
