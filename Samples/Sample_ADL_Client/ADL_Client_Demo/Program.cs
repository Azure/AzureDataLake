using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

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
            //opts.FilterSubmitterContains = "saveenr";
            opts.FilterResult  = new List<JobResult> { JobResult.Cancelled};
            opts.FilterState = new List<JobState> { JobState.Ended};

            opts.FilterDegreeOfParallelism = 1;
            var jobs = client.GetJobList(opts);

            foreach (var job in jobs)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("DOP={0}", job.DegreeOfParallelism);
                Console.WriteLine("Result={0}", job.Result);
                Console.WriteLine("Name={0}", job.Name);
                Console.WriteLine("Result={0}", job.Result);
                Console.WriteLine("Submitter={0}", job.Submitter);
            }
        }

    }
}
