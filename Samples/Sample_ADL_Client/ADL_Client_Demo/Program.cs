using System;
using System.Linq;
using AzureDataLake.Analytics;

namespace ADL_Client_Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var auth_session = new AzureDataLake.Authentication.AuthenticatedSession("ADL_Demo_Client");
            auth_session.Authenticate();

            var client = new AzureDataLake.Analytics.AnalyticsJobClient("datainsightsadhoc", auth_session);

            var opts =new AzureDataLake.Analytics.GetJobListOptions();
            opts.Top = 20;
            //opts.FilterSubmitter = "srevanka@microsoft.com";
            opts.FilterNameContains= "AllJobsHourlyForAdHoc";
            var jobs = client.GetJobList(opts);

            foreach (var job in jobs)
            {
                Console.WriteLine("{0} {1}",job.Name, job.Submitter);
            }
        }

    }
}
