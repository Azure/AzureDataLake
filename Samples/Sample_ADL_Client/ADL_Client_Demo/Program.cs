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
            //opts.FilterResult  = new JobResult[] { JobResult.Cancelled};
            opts.FilterState = new List<JobState> { JobState.Ended};
            var jobs = client.GetJobList(opts);

            foreach (var job in jobs)
            {
                Console.WriteLine("{0} {1} {2}", job.Result, job.Name, job.Submitter);
            }
        }

    }
}
