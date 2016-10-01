using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.Rest.Azure;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsClient : AccountClientBase
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
            foreach (var cur_page in RESTUtil.EnumPages<JobInformation>(page, p => this._adla_client.Job.ListNext(p.NextPageLink)))
            {
                yield return cur_page;
            }
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
}