using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Analytics
{
    [TestClass]
    public class Analytics_Job_Tests : Base_Tests
    {

        [TestMethod]
        public void Verify_Paging_1()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 100;

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            Assert.AreEqual(100,jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_300()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize;

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            Assert.AreEqual(AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize, jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_400()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize + (AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize/2);

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            Assert.IsTrue(jobs.Count>= getjobs_options.Top);
        }


        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 30;
            getjobs_options.OrderByField = AzureDataLake.Analytics.JobOrderByField.DegreeOfParallelism;
            getjobs_options.OrderByDirection = AzureDataLake.Analytics.JobOrderByDirection.Descending;

            foreach (var page in this.adla_job_client.GetJobList(getjobs_options))
            {
                foreach (var job in page)
                {
                    //var job2 = AnalyticsClient.GetJob(job.JobId.Value);
                    System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
                }
            }
        }

        public IEnumerable<T> FlattenArrays<T>(IEnumerable<IEnumerable<T>> arrays)
        {
            foreach (var array in arrays)
            {
                foreach (var item in array)
                {
                    yield return item;
                }
            }
        }

        [TestMethod]
        public void Verify_No_Dupes_in_Paging()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();

            var pages = this.adla_job_client.GetJobList(getjobs_options).Take(3).ToList();
            var count1 = pages.Select(p => p.Length).Sum();
            var items_raw = FlattenArrays(pages).ToList();

            var count2 = items_raw.Count;

            var items_unique = items_raw.Select(i => i.JobId).Distinct().ToList();
            var count3 = items_unique.Count;

            Assert.AreEqual(count1, count2);
            Assert.AreEqual(count2, count3);
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
        public void List_Jobs_Ended()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 30;
            getjobs_options.FilterState = new JobState[] {JobState.Ended};

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Ended,job.State);
                }
            }
        }

        [TestMethod]
        public void List_Jobs_Running()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 30;
            getjobs_options.FilterState = new JobState[] { JobState.Running };

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Running, job.State);
                }
            }
        }

        [TestMethod]
        public void List_Jobs_Ended_Failed()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 30;
            getjobs_options.FilterState = new JobState[] { JobState.Ended };
            getjobs_options.FilterResult= new JobResult[] { JobResult.Failed };

            var jobs = this.adla_job_client.GetJobListUnpaged(getjobs_options);
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Ended, job.State);
                    Assert.AreEqual(JobResult.Failed, job.Result);
                }
            }
        }
    }
}
