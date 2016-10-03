using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class Analytics_Tests : Base_Tests
    {


        [TestMethod]
        public void List_ADLA_Accounts()
        {
            this.Initialize();
            var adla_accounts = this.adla_mgmt_client.ListAccounts();
            foreach (var a in adla_accounts)
            {
                System.Console.WriteLine("Analytics {0} ", a.Name);
            }

        }

        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListPagedOptions();
            getjobs_options.Top = 30;
            getjobs_options.OrderByField = AzureDataLake.Analytics.JobOrderByField.DegreeOfParallelism;
            getjobs_options.OrderByDirection = AzureDataLake.Analytics.JobOrderByDirection.Descending;
            var jobs = this.adla_job_client.GetJobList(getjobs_options).ToArray();

            foreach (var job in jobs)
            {
                //var job2 = AnalyticsClient.GetJob(job.JobId.Value);
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
            }
        }


        [TestMethod]
        public void Submit_Job_with_Syntax_Error()
        {
            this.Initialize();
            var sjo = new AzureDataLake.Analytics.SubmitJobOptions();
            sjo.ScriptText = "FOOBAR";
            sjo.JobName = "Test Job";
            var ji = this.adla_job_client.SubmitJob(sjo);

            System.Console.WriteLine("{0} {1} {2}", ji.Name, ji.JobId, ji.SubmitTime);

            var ji2 = this.adla_job_client.GetJob(ji.JobId.Value);

            Assert.AreEqual(ji.Name, ji2.Name);

        }


        [TestMethod]
        public void List_Databases()
        {
            this.Initialize();
            foreach (var page in this.adla_job_client.ListDatabases())
            {
                foreach (var db in page)
                {
                    System.Console.WriteLine("DB {0}",db.Name);
                }
            }
        }

    }

}
