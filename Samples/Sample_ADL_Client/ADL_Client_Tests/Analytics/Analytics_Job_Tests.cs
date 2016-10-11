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

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
            Assert.AreEqual(100,jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_300()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize;

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
            Assert.AreEqual(AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize, jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_400()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            var top = AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize + (AzureDataLake.Analytics.AnalyticsJobClient.ADLJobPageSize/2);
            getjobs_options.Top = top;

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
            Assert.AreEqual(top,jobs.Count);
        }


        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListOptions();
            getjobs_options.Top = 30;
            getjobs_options.OrderByField = AzureDataLake.Analytics.JobOrderByField.DegreeOfParallelism;
            getjobs_options.OrderByDirection = AzureDataLake.Analytics.JobOrderByDirection.Descending;

            foreach (var job in this.adla_job_client.GetJobListPaged(getjobs_options))
            {
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
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
            getjobs_options.FilterState = new List<JobState> {JobState.Ended};

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
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
            getjobs_options.FilterState = new List<JobState> { JobState.Running };

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
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
            getjobs_options.FilterState = new List<JobState> { JobState.Ended };
            getjobs_options.FilterResult= new List<JobResult> { JobResult.Failed };

            var jobs = this.adla_job_client.GetJobListPaged(getjobs_options).ToList();
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
