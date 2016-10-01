using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsClient : ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsJobManagementClient _adla_client;

        public AnalyticsClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            if (this._adla_client == null)
            {
                this._adla_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(this.AuthenticatedSession.Credentials);
            }
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation> GetJobList(GetJobListPagedOptions options)
        {

            foreach (var page in GetJobListPaged(options))
            {
                foreach (var job in page)
                {
                    yield return job;
                }
            }
        }


        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_client.Job.Get(this.Account, jobid);
            return job;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation[]> GetJobListPaged(GetJobListPagedOptions options)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<JobInformation>();
            
            string @select = null;
            bool? count = null;
            string search = null;
            string format = null;

            if (options.Top > 0)
            {
                oDataQuery.Top = options.Top;
                if (options.OrderByField != null)
                {
                    oDataQuery.OrderBy = string.Format("{0} {1}", options.OrderByField, options.OrderByDirection);
                }
            }

            // Handle the initial response
            var page = this._adla_client.Job.List(this.Account, oDataQuery, @select, count, search, format);
            var jobs_a = job_page_to_array(page);
            yield return jobs_a;

            // While there are additional pages left fetch them
            while (!string.IsNullOrEmpty(page.NextPageLink))
            {
                var jobs = job_page_to_array(page);

                yield return jobs;
                page = this._adla_client.Job.ListNext(page.NextPageLink);
            }
        }

        private static JobInformation[] job_page_to_array(Microsoft.Rest.Azure.IPage<JobInformation> page)
        {
            int num_jobs_in_page = page.Count();
            var jobs = new ADL.Analytics.Models.JobInformation[num_jobs_in_page];

            int i = 0;
            foreach (var job in page)
            {
                jobs[i] = job;
                i++;
            }
            return jobs;
        }

        public ADL.Analytics.Models.JobInformation  SubmitJob(SubmitJobOptions options)
        {
            if (options.JobID == default(System.Guid))
            {
                options.JobID = System.Guid.NewGuid();
            }

            if (options.JobName == null)
            {
                options.JobName = "Job_" + System.DateTime.Now.ToString();
            }

            var jobprops = new USqlJobProperties();
            jobprops.Script = options.ScriptText;

            var jobType = JobType.USql;
            int priority = 1;
            int dop = 1;
            string submitter = null;

            var parameters = new JobInformation(
                name: options.JobName, 
                type: jobType, 
                properties: jobprops,               
                priority: priority, 
                degreeOfParallelism: dop, 
                jobId: options.JobID);
            
            var jobInfo = this._adla_client.Job.Create(this.Account, options.JobID, parameters);

            return jobInfo;
        }
    }

    public class SubmitJobOptions
    {
        public System.Guid JobID;
        public string JobName;
        public string ScriptText;
    }
}