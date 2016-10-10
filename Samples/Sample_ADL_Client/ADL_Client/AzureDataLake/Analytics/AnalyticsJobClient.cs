using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsJobClient : AccountClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsJobManagementClient _adla_job_rest_client;

        public AnalyticsJobClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_job_rest_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_job_rest_client.Job.Get(this.Account, jobid);

            return job;
        }

        public List<ADL.Analytics.Models.JobInformation> GetJobListUnpaged(GetJobListOptions options, int count)
        {
            var results = new List<ADL.Analytics.Models.JobInformation>();
            if (count < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(count));
            }
            else if (count > 300)
            {
                options.Top = 300;
            }
            else
            {
                options.Top = count;
            }

            int actual_count = 0;
            foreach (var page in this.GetJobList(options))
            {
                results.AddRange(page);
                actual_count += page.Length;

                if (actual_count >= count)
                {
                    break;
                }
            }

            return results;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation[]> GetJobList(GetJobListOptions options)
        {
            
            string @select = null;
            bool? count = null;
            string search = null;
            string format = null;

            // Construct OData query from options
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<JobInformation>();
            if (options.Top > 0)
            {
                odata_query.Top = options.Top;
            }
            odata_query.OrderBy = options.CreateOrderByString();
            odata_query.Filter = options.CreateFilterString();

            // Handle the initial response
            var page = this._adla_job_rest_client.Job.List(this.Account, odata_query, @select, count, search, format);
            foreach (var cur_page in RESTUtil.EnumPages<JobInformation>(page, p => this._adla_job_rest_client.Job.ListNext(p.NextPageLink)))
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
                options.JobName = "ADL_Demo_Client_Job_" + System.DateTime.Now.ToString();
            }

            var parameters = CreateNewJobProperties(options);
            var jobInfo = this._adla_job_rest_client.Job.Create(this.Account, options.JobID, parameters);

            return jobInfo;
        }

        private static JobInformation CreateNewJobProperties(SubmitJobOptions options)
        {
            var jobprops = new USqlJobProperties();
            jobprops.Script = options.ScriptText;

            var jobType = JobType.USql;
            int priority = 1;
            int dop = 1;

            var parameters = new JobInformation(
                name: options.JobName,
                type: jobType,
                properties: jobprops,
                priority: priority,
                degreeOfParallelism: dop,
                jobId: options.JobID);
            return parameters;
        }


        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            var stats = this._adla_job_rest_client.Job.GetStatistics(this.Account, jobid);
            return stats;
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            var jobdatapath = this._adla_job_rest_client.Job.GetDebugDataPath(this.Account, jobid);
            return jobdatapath;
        }
    }
}